using Blazor_Training.Domain;
using Blazor_Training.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor_Training.Application
{
    /// <summary>
    /// The core orchestrator for Employee data. 
    /// This service acts as the bridge between the SQL Database (AppDbContext), 
    /// the Tax Math Engine (IPayrollCalculatorService), and the UI.
    /// </summary>
    public class EmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IPayrollCalculatorService _taxEngine;
        private readonly SettingsService _settings;

        /// <summary>
        /// Constructor using Dependency Injection. 
        /// Blazor automatically provides the database, tax engine, and settings when this service is created.
        /// </summary>
        public EmployeeService(AppDbContext context, IPayrollCalculatorService taxEngine, SettingsService settings)
        {
            _context = context;
            _taxEngine = taxEngine;
            _settings = settings;
        }

        // ==========================================
        // STANDARD DATABASE CRUD METHODS
        // ==========================================

        /// <summary>
        /// Retrieves all employees directly from the database without any tax calculations.
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            // .ToList() executes the SQL query and brings the data into server memory
            return _context.Employees.ToList();
        }

        /// <summary>
        /// Finds a specific employee by their unique Guid.
        /// </summary>
        public Employee GetEmployee(Guid id)
        {
            // Searches the database for a matching ID. If not found, returns a blank Employee to prevent crashes.
            return _context.Employees.FirstOrDefault(e => e.Id == id) ?? new Employee();
        }

        /// <summary>
        /// Adds a newly created employee to the database tracking and saves the changes.
        /// </summary>
        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges(); // Commits the INSERT statement to SQL
        }

        /// <summary>
        /// Updates an existing employee's details in the database.
        /// </summary>
        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges(); // Commits the UPDATE statement to SQL
        }

        /// <summary>
        /// Permanently removes an employee from the database.
        /// </summary>
        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges(); // Commits the DELETE statement to SQL
        }

        /// <summary>
        /// Filters the database to find employees whose names contain the search text.
        /// </summary>
        public List<Employee> SearchEmployees(string searchText)
        {
            // If the search box is empty, just return everyone
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return GetAllEmployees();
            }

            // Uses LINQ to filter names, converting both to lowercase so the search is case-insensitive
            return _context.Employees
                .Where(e => e.Name.ToLower().Contains(searchText.ToLower()))
                .ToList();
        }

        // ==========================================
        // ADVANCED TAX CALCULATION ORCHESTRATORS
        // ==========================================

        /// <summary>
        /// Retrieves all employees, calculates their precise Kenyan taxes using the live settings, 
        /// and attaches the resulting Payslip to their temporary 'CurrentPayslip' property.
        /// </summary>
        public async Task<List<Employee>> GetAllEmployeesWithCalculatedPayAsync()
        {
            // 1. Get raw employees from the DB
            var employees = GetAllEmployees();

            // 2. Fetch the current, active tax laws from the database asynchronously
            var currentLaw = await _settings.GetCurrentConfigAsync();

            // 3. Loop through every employee, run the math, and pack their 'backpack'
            foreach (var emp in employees)
            {
                emp.CurrentPayslip = _taxEngine.GeneratePayslip(emp, currentLaw);
            }

            // 4. Return the fully loaded list to the UI so it can display the Net Pay
            return employees;
        }

        /// <summary>
        /// Performs a search for specific employees and calculates the taxes just for those results.
        /// </summary>
        public async Task<List<Employee>> SearchEmployeesWithCalculatedPayAsync(string searchText)
        {
            // 1. Get the filtered list of employees based on the search box
            var employees = SearchEmployees(searchText);

            // 2. Fetch the current tax laws
            var currentLaw = await _settings.GetCurrentConfigAsync();

            // 3. Calculate taxes for just this filtered group
            foreach (var emp in employees)
            {
                emp.CurrentPayslip = _taxEngine.GeneratePayslip(emp, currentLaw);
            }

            // 4. Return the filtered, fully calculated list to the UI
            return employees;
        }
    }
}