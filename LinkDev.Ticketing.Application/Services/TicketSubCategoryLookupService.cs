using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace LinkDev.Ticketing.Application.Services
{
    public class TicketSubCategoryLookupService : LookupService<LookupDTO>
    {
        private readonly IRepository<TicketSubCategory> _repository;
        public TicketSubCategoryLookupService(IRepository<TicketSubCategory> repository, IMemoryCache memoryCache) : base(memoryCache)
        {
            _repository = repository;
        }

        public override IEnumerable<LookupDTO> GetAll()
        {
            //return _cache.GetOrCreate(id, entry =>
            //{
            //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            //    entry.SlidingExpiration = TimeSpan.FromMinutes(2);
            //    entry.Priority = CacheItemPriority.High;

            //    return $"Product-{id}";
            //});

            return _repository.GetAll().Select(x => new LookupDTO
            {
                Code = x.Code,
                Name = x.Name,
                LangId = x.LangId
            });
        }
    }
}