using Blazor_Training.Domain;
using Blazor_Training.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Training.Application
{
    public class SettingsService
    {
        private readonly AppDbContext _context;

        public SettingsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StatutoryConfig> GetCurrentConfigAsync()
        {
            // Try to find the existing config in the database
            var config = await _context.StatutoryConfigs.FirstOrDefaultAsync();

            // If the database is completely empty, seed the defaults so it always exists!
            if (config == null)
            {
                config = new StatutoryConfig
                {
                    PersonalRelief = 2400m,
                    NssfUpperLimit = 18000m,
                    ShaRate = 0.0275m,
                    HousingLevy = 0.015m
                };
                _context.StatutoryConfigs.Add(config);
                await _context.SaveChangesAsync();
            }

            return config;
        }

        public async Task SaveConfigAsync(StatutoryConfig config)
        {
            _context.StatutoryConfigs.Update(config);
            await _context.SaveChangesAsync();
        }
    }
}