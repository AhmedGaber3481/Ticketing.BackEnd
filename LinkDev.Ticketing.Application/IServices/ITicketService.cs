using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.DTos;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;

namespace LinkDev.Ticketing.Application.IServices
{
    public interface ITicketService
    {
        ListViewResult<TicketView> GetTickets(TicketRequestDTO requestDTO, Guid correlationId, string userId);
        ResponseMessage<bool> SaveTicket(TicketDTO ticketDTO, string culture, Guid correlationId);
        ResponseMessage<TicketDTO> GetTicket(int ticketId, string culture, Guid correlationId);
    }
}
