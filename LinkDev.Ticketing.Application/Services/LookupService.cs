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
                //switch (_lookupType)
                //{
                //    case LookupType.TicketType:
                //        return _lookupRepository.GetLookup<TicketTypeLookup>(lookupType, culture);
                //    case LookupType.TicketCategory:
                //        return _lookupRepository.GetLookup<TicketCategoryLookup>(lookupType, culture);
                //    //case LookupType.TicketSubCategory:
                //    //    return _lookupRepository.GetLookup<TicketSubCategory>(lookupType, culture);
                //    case LookupType.TicketPriority:
                //        return _lookupRepository.GetLookup<TicketPriorityLookup>(lookupType, culture);
                //    case LookupType.TicketStatus:
                //        return _lookupRepository.GetLookup<TicketStatusLookup>(lookupType, culture);
                //    default:
                //        throw new ArgumentException("Invalid lookup type");
                //}
                return _lookupRepository.GetLookup<BaseLookup>(_lookupType, culture);
            }
            else
            {
                throw new ArgumentException("Invalid lookup type");
            }
        }
    }
}