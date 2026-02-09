using System.Linq;
using System.Collections.Generic;
namespace Blazor_Training.Data
{
    public class WorkerService
    {
        private static List<Worker> myWorkers = new List<Worker>();

        public List<Worker> GetAllWorkers()
        {
            return myWorkers;
        }

        public void AddWorker(Worker worker)
        {
            myWorkers.Add(worker);
        }
        public Worker GetWorker(Guid Id)
        {
            return myWorkers.FirstOrDefault(w => w.Id == Id);
        }
        public void DeleteWorker(Worker worker)
        {
            myWorkers.Remove(worker);
        }
    }
}
