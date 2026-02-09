namespace Blazor_Training.Data
{
    public class Worker
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double hourlyPay { get; set; }
        public int hoursWorked { get; set; }
        public double totalSalary => hourlyPay * hoursWorked;

        public Worker() { }

        public Worker(string _name, double _hourlypay, int _hoursworked)
        {
            Name = _name;
            hourlyPay = _hourlypay;
            hoursWorked = _hoursworked;
        }
    }
}
