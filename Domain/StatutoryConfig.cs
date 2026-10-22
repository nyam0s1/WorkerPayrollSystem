using System;
namespace Blazor_Training.Domain;

public class StatutoryConfig
{
    /// <summary>
    /// Monthly defaults for a fresh installation, using the NSSF Year 4 rates
    /// effective from February 2026. Existing saved settings remain unchanged.
    /// </summary>
    public static StatutoryConfig CreateDefault() => new()
    {
        PersonalRelief = 2400m,
        NssfLowerRate = 0.06m,
        NssfLowerLimit = 9000m,
        NssfUpperLimit = 6480m,
        ShaRate = 0.0275m,
        HousingLevy = 0.015m
    };

    //primary key for the db
    public Guid Id { get; set; } = Guid.NewGuid();
    //PAYE rates
    public decimal PayeRate { get; set; }
    //SHA rate 
    public decimal ShaRate { get; set; }
    //NSSF rate
    public decimal NssfLowerRate { get; set; }
    //NSSF Limits
    //Lower Limit:
    public decimal NssfLowerLimit { get; set; }
    // Maximum employee contribution, not the pensionable earnings ceiling.
    public decimal NssfUpperLimit { get; set; }
    //Housing Levy rate
    public decimal HousingLevy { get; set; }
    //personal relief amount
    public decimal PersonalRelief { get; set; }
}
