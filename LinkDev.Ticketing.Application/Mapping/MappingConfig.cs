using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.DTos;
using LinkDev.Ticketing.Domain.Entities;
using Mapster;

namespace LinkDev.Ticketing.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Ticket, TicketDTO>.NewConfig();
            TypeAdapterConfig<TicketDTO, Ticket>.NewConfig();

            // already handled by mapster no need to add configuration
            //TypeAdapterConfig<TicketCategory, LookupDTO>.NewConfig();
            //TypeAdapterConfig<TicketPriority, LookupDTO>.NewConfig();
            //TypeAdapterConfig<TicketStatus, LookupDTO>.NewConfig();
            //TypeAdapterConfig<TicketSubCategory, LookupDTO>.NewConfig()
            //    .Map(dest => dest.Code, src => src.Code)
            //    .Map(dest => dest.Name, src => src.Name);
            //TypeAdapterConfig<TicketType, LookupDTO>.NewConfig();

        }
    }
}
