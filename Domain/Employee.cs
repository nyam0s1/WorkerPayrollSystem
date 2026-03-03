using System.ComponentModel.DataAnnotations;

namespace Blazor_Training.Domain
{
    /// <summary>
    /// Represents a unified Employee entity that supports both 
    /// hourly wage tracking and fixed Kenyan statutory payroll.
    /// </summary>
    public class Employee
    {
        // ==========================================
        // 1. CORE IDENTITY
        // ==========================================

        /// <summary>
        /// Unique identifier for the database.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The full name of the employee (used by the existing UI).
        /// </summary>
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; } = "";


        // ==========================================
        // 2. STATUTORY IDENTIFIERS (KENYA)
        // ==========================================

        /// <summary>
        /// Kenya Revenue Authority PIN, critical for PAYE processing.
        /// </summary>
        public string KraPin { get; set; } = "";

        /// <summary>
        /// National Social Security Fund Number.
        /// </summary>
        public string NssfNumber { get; set; } = "";

        /// <summary>
        /// National Health Insurance Fund (or SHIF) Number.
        /// </summary>
        public string NhifNumber { get; set; } = "";


        // ==========================================
        // 3. EXISTING HOURLY WAGE LOGIC
        // ==========================================

        [Required(ErrorMessage = "Hourly Pay is required!")]
        [Range(0, 1000001, ErrorMessage = "Pay should be between 0 and 1,000,000")]
        public double? hourlyPay { get; set; }

        [Required(ErrorMessage = "Hours Worked are required!")]
        [Range(0, 120, ErrorMessage = "Hours worked should be between 0 and 120")]
        public int? hoursWorked { get; set; }

        /// <summary>
        /// Calculates any hours worked over the standard 40-hour limit.
        /// </summary>
        public int overTimeHours
        {
            get
            {
                int h = hoursWorked.GetValueOrDefault();
                return h > 40 ? h - 40 : 0;
            }
        }

        /// <summary>
        /// Calculates total gross wage based on hourly rate and overtime (1.5x).
        /// </summary>
        public double totalSalary
        {
            get
            {
                double h = hoursWorked.GetValueOrDefault();
                double r = hourlyPay.GetValueOrDefault();
                const int regularLimit = 40;

                // Standard pay
                if (h <= regularLimit)
                {
                    return h * r;
                }

                // Overtime pay
                double regularPay = regularLimit * r;
                double overtimePay = overTimeHours * (r * 1.5);
                return regularPay + overtimePay;
            }
        }


        // ==========================================
        // 4. NEW FIXED SALARY & ALLOWANCES 
        // ==========================================

        /// <summary>
        /// Fixed monthly salary for permanent employees (used by the tax engine).
        /// </summary>
        public decimal BasicSalary { get; set; }

        /// <summary>
        /// Taxable house allowance.
        /// </summary>
        public decimal HouseAllowance { get; set; }

        /// <summary>
        /// Taxable transport allowance.
        /// </summary>
        public decimal TransportAllowance { get; set; }


        // ==========================================
        // 5. TAX EXEMPTION STATUS
        // ==========================================

        /// <summary>
        /// Used to determine if the employee qualifies for insurance relief.
        /// </summary>
        public bool IsMarried { get; set; }

        /// <summary>
        /// Used to apply the KRA disability tax exemption (up to 150k).
        /// </summary>
        public bool HasDisability { get; set; }


        // ==========================================
        // CONSTRUCTORS
        // ==========================================

        public Employee() { }

        public Employee(string _name, double _hourlypay, int _hoursworked)
        {
            Name = _name;
            hourlyPay = _hourlypay;
            hoursWorked = _hoursworked;
        }
    }
}