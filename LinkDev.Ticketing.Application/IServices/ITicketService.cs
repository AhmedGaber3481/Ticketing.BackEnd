using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.DTos;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;

namespace LinkDev.Ticketing.Application.IServices
{
    public interface ITicketService
    {
        TicketSearchResult<TicketView> GetTickets(TicketRequestDTO requestDTO);
        ResponseMessage<bool> SaveTicket(TicketDTO ticketDTO, Guid correlationId);
    }
}
