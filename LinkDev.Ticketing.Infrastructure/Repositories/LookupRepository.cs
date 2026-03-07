using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;
using LinkDev.Ticketing.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace LinkDev.Ticketing.Infrastructure.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private readonly TicketingContext _ticketingContext;
        private readonly IMemoryCache _memoryCache;
        public LookupRepository(TicketingContext ticketingContext, IMemoryCache memoryCache)
        {
            _ticketingContext = ticketingContext;
            _memoryCache = memoryCache;
        }

        public List<LookupDTO> GetLookup<T>(string lookupType, string culture) where T : BaseLookup
        {
            string cacheKey = $"{lookupType}_{culture}";
            if (!_memoryCache.TryGetValue(cacheKey, out List<LookupDTO>? lookupItems))
            {
                lookupItems = GetAll<T>(culture);
                _memoryCache.Set(cacheKey, lookupItems, TimeSpan.FromHours(1));
            }
            return lookupItems ?? new List<LookupDTO>();
        }

        private List<LookupDTO> GetAll<T>(string culture) where T : BaseLookup
        {
            short langId = culture.ToLower() == "en-us" ? (short)1 : (short)2;
            var lookupItems = _ticketingContext.Set<T>().Where(x => !x.IsDeleted && x.LangId == langId)
                .AsEnumerable()
                .Select(x => x.ToLookupDTO())
                .ToList();
            return lookupItems;
        }

        public int? GetLookupItemId<T>(LookupType lookupType, string itemCode, string culture) where T : BaseLookup
        {
            var list = GetLookup<T>(lookupType.ToString(), culture);
            if(list != null)
            {
                return list.FirstOrDefault(x => x.Code ==  itemCode)?.Id;
            }
            return null;
        }
    }
}
