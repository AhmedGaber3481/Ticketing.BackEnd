using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Application.IServices
{
    public abstract class LookupService<LookupDTO>
    {
        private IMemoryCache _memoryCache;
        //protected readonly string _currentCulture;
        protected LookupService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            //_currentCulture = currentCulture;
        }
        public abstract IEnumerable<LookupDTO> GetAll(string culture);

        public IEnumerable<LookupDTO>? GetLookup(string lookupKey, string culture)
        {
            return _memoryCache.GetOrCreate(lookupKey + "_" + culture, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                entry.SlidingExpiration = TimeSpan.FromMinutes(20);
                entry.Priority = CacheItemPriority.High;

                return GetAll(culture);
            });
        }
    }
}
