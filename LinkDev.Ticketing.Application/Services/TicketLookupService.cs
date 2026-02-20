using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace LinkDev.Ticketing.Application.Services
{
    public class TicketLookupService<T> : LookupService<LookupDTO> where T : BaseLookup
    {
        private readonly IRepository<T> _repository;
        public TicketLookupService(IRepository<T> repository, IMemoryCache memoryCache) : base(memoryCache)
        {
            _repository = repository;
        }

        public override IEnumerable<LookupDTO> GetAll(string culture)
        {
            short langId = culture.ToLower() == "en-us" ? (short)1 : (short)2;
            return _repository.Where(x => !x.IsDeleted && x.LangId == langId).Select(x => x.ToLookupDTO());
        }
    }
}