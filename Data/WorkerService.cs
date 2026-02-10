using Microsoft.EntityFrameworkCore; // Needed for database commands

namespace Blazor_Training.Data
{
    public class WorkerService
    {
        // We no longer use a "static List". We use the Database Context.
        private readonly AppDbContext _context;

        // Constructor: We ask the app to give us the database connection
        public WorkerService(AppDbContext context)
        {
            _context = context;
        }

        // READ: Get all workers from the DB table
        public List<Worker> GetAllWorkers()
        {
            return _context.Workers.ToList();
        }

        // CREATE: Add a new worker to the DB
        public void AddWorker(Worker worker)
        {
            _context.Workers.Add(worker);
            _context.SaveChanges(); // This commits the change to the database
        }

        // UPDATE: Save changes to an existing worker
        // (We didn't need this before because memory lists update automatically. DBs don't!)
        public void UpdateWorker(Worker worker)
        {
            _context.Workers.Update(worker);
            _context.SaveChanges();
        }

        // READ: Find a single worker by ID
        public Worker GetWorker(Guid Id)
        {
            return _context.Workers.FirstOrDefault(w => w.Id == Id);
        }

        // DELETE: Remove a worker from the DB
        public void DeleteWorker(Worker worker)
        {
            _context.Workers.Remove(worker);
            _context.SaveChanges();
        }
    }
}