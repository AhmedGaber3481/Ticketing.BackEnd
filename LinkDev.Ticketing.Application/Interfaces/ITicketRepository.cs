using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Application.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        IEnumerable<TicketView> GetTickets(TicketRequestDTO requestDTO, out int totalCount);
    }
}
