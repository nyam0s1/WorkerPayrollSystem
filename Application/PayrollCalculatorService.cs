using Blazor_Training.Domain;
namespace Blazor_Training.Application;

public interface IPayrollCalculatorService
{
    Payslip GeneratePayslip(Employee employee, StatutoryConfig config);
}

public class PayrollCalculatorService : IPayrollCalculatorService
{
    public Payslip GeneratePayslip(Employee employee, StatutoryConfig config)
    {
        Payslip slip = new Payslip();

        slip.EmployeeId = employee.Id;

        slip.BasicSalary = employee.BasicSalary;
        slip.HouseAllowance = employee.HouseAllowance;
        slip.TransportAllowance = employee.TransportAllowance;

        slip.GrossPay = slip.BasicSalary + slip.HouseAllowance + slip.TransportAllowance;

        decimal Nssfamount = slip.GrossPay * 0.06m;
        if (Nssfamount > config.NssfUpperLimit)
        {
            slip.NssfAmount = config.NssfUpperLimit;
        }
        else
        {
            slip.NssfAmount = Nssfamount;
        }

        slip.TaxablePay = slip.GrossPay - slip.NssfAmount;

        slip.PayeAmount = CalculatePaye(slip.TaxablePay, config.PersonalRelief);

        slip.ShaAmount = slip.GrossPay * config.ShaRate;

        slip.HousingLevyAmount = slip.GrossPay * config.HousingLevy;

        slip.NetPay = slip.TaxablePay - (slip.PayeAmount + slip.ShaAmount + slip.HousingLevyAmount);

        return slip;
    }

    private decimal CalculatePaye(decimal taxablePay, decimal personalRelief)
    {
        decimal tax = 0m;

        // The Kenyan Tax Bands (2025/2026 rules):
        // 1. First 24,000 @ 10%
        // 2. Next 8,333 @ 25%
        // 3. Next 467,667 @ 30%
        // 4. Next 300,000 @ 32.5%
        // 5. Above 800,000 @ 35%

        // 5. Above 800,000
        if (taxablePay > 800000m)
        {
            // They pay the max tax of all previous bands, PLUS 35% of anything over 800k
            tax = 242283.35m + ((taxablePay - 800000m) * 0.35m);
        }
        // 4. Up to 800,000
        else if (taxablePay > 500000m)
        {
            // They pay the max tax of Bands 1, 2, and 3, PLUS 32.5% of anything over 500k
            tax = 144783.35m + ((taxablePay - 500000m) * 0.325m);
        }
        // 3. Up to 500,000
        else if (taxablePay > 32333m)
        {
            // Max tax of Bands 1 and 2, PLUS 30% of anything over 32,333
            tax = 4483.25m + ((taxablePay - 32333m) * 0.30m);
        }
        // 2. Up to 32,333
        else if (taxablePay > 24000m)
        {
            // Max tax of Band 1, PLUS 25% of anything over 24k
            tax = 2400m + ((taxablePay - 24000m) * 0.25m);
        }
        // 1. Up to 24,000
        else if (taxablePay > 0m)
        {
            tax = taxablePay * 0.10m;
        }

        // Subtract relief
        decimal finalPaye = tax - personalRelief;

        // PAYE cannot be negative
        if (finalPaye < 0)
        {
            return 0m;
        }

        return finalPaye;
    }
}
