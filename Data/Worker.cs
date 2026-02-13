using System.ComponentModel.DataAnnotations;

namespace Blazor_Training.Data
{
    public class Worker
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Hourly Pay is required!")]
        [Range(0, 1000001, ErrorMessage = "Pay should be between 0 and 1 000 000 ")]
        public double? hourlyPay { get; set; }
        [Required(ErrorMessage = "Hours Worked are required!")]
        [Range(0, 120, ErrorMessage = "Hours worked should be between 0 and 120")]
        public int? hoursWorked { get; set; }

        public Worker() { }

        public Worker(string _name, double _hourlypay, int _hoursworked)
        {
            Name = _name;
            hourlyPay = _hourlypay;
            hoursWorked = _hoursworked;
        }

        public int overTimeHours
        {
            get
            {
                // If value is null, treat it as 0
                int h = hoursWorked.GetValueOrDefault();
                if (h > 40)
                {
                    return h - 40;
                }
                return 0;
            }
        }

        public double totalSalary
        {
            get
            {
                // If value is null, treat it as 0
                double h = hoursWorked.GetValueOrDefault();
                double r = hourlyPay.GetValueOrDefault();
                const int regularLimit = 40;

                if (h <= regularLimit)
                {
                    return h * r;
                }
                else
                {
                    double regularPay = regularLimit * r;
                    double overtimePay = overTimeHours * (r * 1.5);

                    return regularPay + overtimePay;
                }
            }

        }
        public double NetSalary
        {
            get
            {
                // Subtract 30,000 from the total salary
                double net = totalSalary - 30000;
                return net < 0 ? 0 : net; // Optional: prevents negative salary if pay is low
            }
        }
    }
}
