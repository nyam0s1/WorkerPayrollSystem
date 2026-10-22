using Blazor_Training.Domain;
using Blazor_Training.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Training.Application;

public class EmployeeService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IPayrollCalculatorService _taxEngine;
    private readonly SettingsService _settings;

    public EmployeeService(IDbContextFactory<AppDbContext> contextFactory,
        IPayrollCalculatorService taxEngine, SettingsService settings)
    {
        _contextFactory = contextFactory;
        _taxEngine = taxEngine;
        _settings = settings;
    }

    // Each operation owns its context so Blazor circuits never share pending queries
    // or keep an employee's unsaved form edits in the database change tracker.
    public List<Employee> GetAllEmployees()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Employees.AsNoTracking().OrderBy(e => e.Name).ToList();
    }

    public Employee? GetEmployee(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Employees.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public void AddEmployee(Employee employee)
    {
        using var context = _contextFactory.CreateDbContext();
        employee.Name = employee.Name.Trim();
        context.Employees.Add(employee);
        context.SaveChanges();
    }

    public bool UpdateEmployee(Employee employee)
    {
        using var context = _contextFactory.CreateDbContext();
        var existing = context.Employees.Find(employee.Id);
        if (existing == null)
            return false;

        employee.Name = employee.Name.Trim();
        context.Entry(existing).CurrentValues.SetValues(employee);
        context.SaveChanges();
        return true;
    }

    public void DeleteEmployee(Employee employee)
    {
        using var context = _contextFactory.CreateDbContext();
        var existing = context.Employees.Find(employee.Id);
        if (existing == null)
            return;

        context.Employees.Remove(existing);
        context.SaveChanges();
    }

    public List<Employee> SearchEmployees(string searchText)
    {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Employees.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var term = searchText.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Contains(term));
        }

        return query.OrderBy(e => e.Name).ToList();
    }

    public Task<List<Employee>> GetAllEmployeesWithCalculatedPayAsync() =>
        CalculatePayAsync(GetAllEmployees());

    public Task<List<Employee>> SearchEmployeesWithCalculatedPayAsync(string searchText) =>
        CalculatePayAsync(SearchEmployees(searchText));

    public async Task<Employee?> GetEmployeeWithCalculatedPayAsync(Guid id)
    {
        var employee = GetEmployee(id);
        if (employee == null)
            return null;

        var currentLaw = await _settings.GetCurrentConfigAsync();
        employee.CurrentPayslip = _taxEngine.GeneratePayslip(employee, currentLaw);
        return employee;
    }

    private async Task<List<Employee>> CalculatePayAsync(List<Employee> employees)
    {
        var currentLaw = await _settings.GetCurrentConfigAsync();
        foreach (var employee in employees)
            employee.CurrentPayslip = _taxEngine.GeneratePayslip(employee, currentLaw);

        return employees;
    }
}
