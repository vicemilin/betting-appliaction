using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Repository;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Settings;
using Microsoft.Extensions.Options;

namespace VM.BettingApplication.Core.Services.Implementation
{
    public class SportService : ISportService
    {
        private const string AllSportsCacheKey = "all_sports";

        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationSettings _appSettings;

        public SportService(IMemoryCache memoryCache,IOptions<ApplicationSettings> appSettings)
        {
            _memoryCache = memoryCache;
            _appSettings = appSettings.Value;
        }

        public async Task<Sport[]> GetAllSports()
        {
            if(!_memoryCache.TryGetValue(AllSportsCacheKey, out Sport[] sports))
            {
                using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
                {
                    sports = await databaseContext.Sports.ToArrayAsync();
                    _memoryCache.Set(AllSportsCacheKey, sports, TimeSpan.FromDays(1));
                }
            }

            return sports;
        }
    }
}

