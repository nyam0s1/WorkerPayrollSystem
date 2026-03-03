using Blazor_Training.Domain;
using Blazor_Training.Infrastructure;
using Microsoft.EntityFrameworkCore; // Needed for database commands

namespace Blazor_Training.Application
{
    public class EmployeeService
    {
        // We no longer use a "static List". We use the Database Context.
        private readonly AppDbContext _context;

        // Constructor: We ask the app to give us the database connection
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        // READ: Get all workers from the DB table
        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public List<Employee> SearchWorkers(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return GetAllEmployees();
            }
            else
            {
                return _context.Employees
                    .Where(w => w.Name.Contains(searchText))
                    .ToList();
            }
        }

        // CREATE: Add a new Employee to the DB
        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges(); // This commits the change to the database
        }

        // UPDATE: Save changes to an existing worker
        // (We didn't need this before because memory lists update automatically. DBs don't!)
        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        // READ: Find a single Employee by ID
        public Employee GetEmployee(Guid Id)
        {
            return _context.Employees.FirstOrDefault(w => w.Id == Id);
        }

        // DELETE: Remove a Employee from the DB
        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}