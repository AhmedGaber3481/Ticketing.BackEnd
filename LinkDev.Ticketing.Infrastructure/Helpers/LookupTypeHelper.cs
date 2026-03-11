using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;
using LinkDev.Ticketing.Infrastructure.Data;

namespace LinkDev.Ticketing.Infrastructure.Helpers
{
    public class LookupTypeHelper
    {
        public static IQueryable<BaseLookup> GetLookupSource(LookupType lookupType, TicketingContext ticketingContext)
        {
            switch (lookupType)
            {
                case LookupType.TicketType:
                    return ticketingContext.Set<TicketTypeLookup>();

                case LookupType.TicketCategory:
                    return ticketingContext.Set<TicketCategoryLookup>();

                case LookupType.TicketPriority:
                    return ticketingContext.Set<TicketPriorityLookup>();

                case LookupType.TicketStatus:
                    return ticketingContext.Set<TicketStatusLookup>();

                default:
                    throw new ArgumentException("Invalid lookup type");
            }
        }
    }
}
