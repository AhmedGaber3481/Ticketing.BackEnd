using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Application.IServices
{
    public abstract class LookupService<LookupDTO>
    {
        private IMemoryCache _memoryCache;
        protected LookupService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public abstract IEnumerable<LookupDTO> GetAll();

        public IEnumerable<LookupDTO>? GetLookup(string lookupKey)
        {
            return _memoryCache.GetOrCreate(lookupKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                entry.SlidingExpiration = TimeSpan.FromMinutes(20);
                entry.Priority = CacheItemPriority.High;

                return GetAll();
            });
        }
    }
}
