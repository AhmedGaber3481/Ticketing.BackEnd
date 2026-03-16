using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;

namespace LinkDev.Ticketing.Application.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        IEnumerable<TicketView> GetTickets(TicketRequestDTO requestDTO, string userId, Guid correlationId, out int totalCount);
        List<TicketStatisticsDTO>? GetTicketStatistics(string culture, string userId, Guid correlationId);
    }
}
