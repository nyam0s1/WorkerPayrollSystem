using Blazor_Training.Domain;
using Blazor_Training.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Training.Application;

public class SettingsService
{
    private static readonly SemaphoreSlim SeedLock = new(1, 1);
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public SettingsService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<StatutoryConfig> GetCurrentConfigAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var config = await context.StatutoryConfigs.AsNoTracking().OrderBy(c => c.Id).FirstOrDefaultAsync();
        if (config != null)
            return config;

        // Prerendering and interactive circuits may both request the first settings.
        await SeedLock.WaitAsync();
        try
        {
            config = await context.StatutoryConfigs.AsNoTracking().OrderBy(c => c.Id).FirstOrDefaultAsync();
            if (config == null)
            {
                config = StatutoryConfig.CreateDefault();
                context.StatutoryConfigs.Add(config);
                await context.SaveChangesAsync();
            }

            return config;
        }
        finally
        {
            SeedLock.Release();
        }
    }

    public async Task SaveConfigAsync(StatutoryConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        if (config.PersonalRelief < 0 || config.NssfUpperLimit < 0)
            throw new ArgumentException("Relief and contribution amounts must be zero or greater.");
        if (config.ShaRate < 0 || config.ShaRate > 1 || config.HousingLevy < 0 || config.HousingLevy > 1)
            throw new ArgumentException("Enter deduction rates as decimals between 0 and 1.");

        await using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.StatutoryConfigs.FindAsync(config.Id);
        if (existing == null)
            throw new InvalidOperationException("These settings no longer exist. Reload the page and try again.");

        context.Entry(existing).CurrentValues.SetValues(config);
        await context.SaveChangesAsync();
    }
}
