using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;

namespace LinkDev.Ticketing.Application.Services
{
    public class LookupService : ILookupService
    {
        private readonly ILookupRepository _lookupRepository;
        public LookupService(ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }
        public IEnumerable<LookupDTO>? GetLookup(string lookupType, string culture)
        {
            if (Enum.TryParse(lookupType, true, out LookupType _lookupType))
            {
                return _lookupRepository.GetLookup<BaseLookup>(_lookupType, culture);
            }
            else
            {
                throw new ArgumentException("Invalid lookup type");
            }
        }
    }
}