using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace LinkDev.Ticketing.Application.Services
{
    public class LookupFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache? _memoryCache;
        public LookupFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _memoryCache = _serviceProvider.GetService(typeof(IMemoryCache)) as IMemoryCache;
        }
        public LookupService<LookupDTO> GetInstance(string lookupType)
        {
            if (Enum.TryParse(lookupType, true, out LookupType _lookupType))
            {
                switch (_lookupType)
                {
                    case LookupType.TicketType:
                        return new TicketTypeLookupService((IRepository<TicketType>)_serviceProvider.GetService(typeof(IRepository<TicketType>))!
                            , _memoryCache!);
                    case LookupType.TicketCategory:
                        return new TicketCategoryLookupService((IRepository<TicketCategory>)_serviceProvider.GetService(typeof(IRepository<TicketCategory>))!,
                            _memoryCache!);
                    case LookupType.TicketSubCategory:
                        return new TicketSubCategoryLookupService((IRepository<TicketSubCategory>)_serviceProvider.GetService(typeof(IRepository<TicketSubCategory>))!,
                            _memoryCache!);
                    case LookupType.TicketPriority:
                        return new TicketPriorityLookupService((IRepository<TicketPriority>)_serviceProvider.GetService(typeof(IRepository<TicketPriority>))!,
                            _memoryCache!);
                    case LookupType.TicketStatus:
                        return new TicketStatusLookupService((IRepository<TicketStatus>)_serviceProvider.GetService(typeof(IRepository<TicketStatus>))!,
                            _memoryCache!);
                    default:
                        throw new ArgumentException("Invalid lookup type");
                }
            }
            throw new ArgumentException("Invalid lookup type");
        }

    }
}
