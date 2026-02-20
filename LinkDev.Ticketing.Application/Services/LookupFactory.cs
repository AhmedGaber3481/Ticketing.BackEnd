using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace LinkDev.Ticketing.Application.Services
{
    public class LookupFactory
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IMemoryCache? _memoryCache;

        public LookupFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //_memoryCache = memoryCache;
        }
        public LookupService<LookupDTO> GetInstance(string lookupType, string currentCulture)
        {
            if (Enum.TryParse(lookupType, true, out LookupType _lookupType))
            {
                switch (_lookupType)
                {
                    case LookupType.TicketType:
                        return GetService<TicketType>();
                    case LookupType.TicketCategory:
                        return GetService<TicketCategory>();
                    case LookupType.TicketSubCategory:
                        return GetService<TicketSubCategory>();
                    case LookupType.TicketPriority:
                        return GetService<TicketPriority>();
                    case LookupType.TicketStatus:
                        return GetService<TicketStatus>();
                    default:
                        throw new ArgumentException("Invalid lookup type");
                }
            }
            throw new ArgumentException("Invalid lookup type");
        }

        private TicketLookupService<T> GetService<T>() where T : BaseLookup
        {
            var service = _serviceProvider.GetService(typeof(TicketLookupService<T>)) as TicketLookupService<T>;
            if (service == null)
            {
                throw new InvalidOperationException($"Repository for type {typeof(T).Name} not found.");
            }
            return service;
        }
    }
}
