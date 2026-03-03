using System;
namespace Blazor_Training.Domain;

public class Payslip
{
    //employee connected to these amounts
    public Guid EmployeeId { get; set; }
    //payroll identification
    public Guid PayrollRunId { get; set; }

    //Salary paid initially
    public decimal BasicSalary { get; set; }
    //Allowances given to the employee; are taxable
    public decimal HouseAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    //Sum of Basic Salary and Allowances
    public decimal GrossPay { get; set; }

    //NSSF
    public decimal NssfAmount { get; set; }
    //Money that will be subject to tax
    public decimal TaxablePay { get; set; }

    //amounts paid to:
    //PAYE
    public decimal PayeAmount { get; set; }
    //SHA
    public decimal ShaAmount { get; set; }
    //Housing Levy
    public decimal HousingLevyAmount { get; set; }

    //amount to be added for tax relief
    public decimal PersonalRelief { get; set; }

    //final amount of pay after deductions and relief
    public decimal NetPay { get; set; }
}
