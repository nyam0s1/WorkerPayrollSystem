using Blazor_Training.Application;
using Blazor_Training.Domain;

var calculator = new PayrollCalculatorService();
var checks = 0;
var failures = 0;
void Check(string name, decimal actual, decimal expected)
{
    checks++;
    var passed = actual == expected;
    Console.WriteLine($"{(passed ? "PASS" : "FAIL")}: {name}; actual={actual:0.00}, expected={expected:0.00}");
    if (!passed) failures++;
}

var currentConfig = new StatutoryConfig
{
    PersonalRelief = 2400m,
    NssfUpperLimit = 6480m,
    ShaRate = 0.0275m,
    HousingLevy = 0.015m
};

// Standard resident employee, monthly pay, no additional reliefs or exemptions.
var slip = calculator.GeneratePayslip(new Employee { BasicSalary = 50000m }, currentConfig);
Check("Monthly NSSF at KES 50,000 gross", slip.NssfAmount, 3000m);
Check("Monthly SHIF at KES 50,000 gross", slip.ShaAmount, 1375m);
Check("Monthly Housing Levy at KES 50,000 gross", slip.HousingLevyAmount, 750m);
Check("Taxable income includes allowable statutory deductions", slip.TaxablePay, 44875m);
Check("PAYE uses income after allowable deductions", slip.PayeAmount, 5845.85m);
Check("Monthly net pay is consistent", slip.NetPay, 39029.15m);

var lowPay = calculator.GeneratePayslip(new Employee { BasicSalary = 5000m }, currentConfig);
Check("SHIF minimum monthly contribution", lowPay.ShaAmount, 300m);

// Fresh repository defaults use the Year 4 employee contribution cap.
var repositoryDefaultConfig = StatutoryConfig.CreateDefault();
var highPay = calculator.GeneratePayslip(new Employee { BasicSalary = 200000m }, repositoryDefaultConfig);
Check("Repository default NSSF cap matches Year 4 employee maximum", highPay.NssfAmount, 6480m);

var hourly = new Employee { hourlyPay = 1000, hoursWorked = 45 };
Check("Hourly wage includes 1.5x overtime above 40 hours", (decimal)hourly.totalSalary, 47500m);

var emptySlip = calculator.GeneratePayslip(new Employee(), currentConfig);
Check("Zero gross has zero net pay", emptySlip.NetPay, 0m);
Check("Zero gross has no monthly SHIF deduction", emptySlip.ShaAmount, 0m);
Check("Low income has no negative PAYE", lowPay.PayeAmount, 0m);
Check("Personal relief is included on payslip", slip.PersonalRelief, 2400m);

foreach (var (taxable, tax) in new (decimal, decimal)[]
{
    (0m, 0m), (24000m, 2400m), (24001m, 2400.25m),
    (32333m, 4483.25m), (32334m, 4483.55m),
    (500000m, 144783.35m), (500001m, 144783.675m),
    (800000m, 242283.35m), (800001m, 242283.70m)
})
{
    // Isolate published PAYE bands: a KES 300 SHIF deduction offsets the
    // extra 300 gross, with other contributions and personal relief zero.
    var bandConfig = new StatutoryConfig();
    var bandSlip = calculator.GeneratePayslip(new Employee { BasicSalary = taxable + 300m }, bandConfig);
    Check($"PAYE band at taxable KES {taxable}", bandSlip.PayeAmount,
        decimal.Round(tax, 2, MidpointRounding.AwayFromZero));
}

Console.WriteLine($"Checks: {checks}; passed: {checks - failures}; failed: {failures}");
return failures == 0 ? 0 : 1;
