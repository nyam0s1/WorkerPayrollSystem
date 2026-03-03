using System;
namespace Blazor_Training.Domain;

public class StatutoryConfig
{
    //PAYE rates
    public decimal PayeRate { get; set; }
    //SHA rate 
    public decimal ShaRate { get; set; }
    //NSSF rate
    public decimal NssfLowerRate { get; set; }
    //NSSF Limits
    //Lower Limit:
    public decimal NssfLowerLimit { get; set; }
    //Lower Limit:
    public decimal NssfUpperLimit { get; set; }
    //Housing Levy rate
    public decimal HousingLevy { get; set; }
    //personal relief amount
    public decimal PersonalRelief { get; set; }
}
