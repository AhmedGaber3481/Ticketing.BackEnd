using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace LinkDev.Ticketing.Application.Services
{
    public class TicketStatusLookupService : LookupService<LookupDTO>
    {
        private readonly IRepository<TicketStatus> _repository;
        public TicketStatusLookupService(IRepository<TicketStatus> repository, IMemoryCache memoryCache) : base(memoryCache)
        {
            _repository = repository;
        }

        public override IEnumerable<LookupDTO> GetAll()
        {
            return _repository.GetAll().Select(x => new LookupDTO
            {
                Code = x.Code,
                Name = x.Name,
                LangId = x.LangId
            });
        }
    }
}