using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketingContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<TicketView> GetTicketViews(short LangCode = 1)
        {
            return _dBContext.Database.SqlQueryRaw<TicketView>("execute GetTickets @Lang", new SqlParameter("@Lang" , LangCode));
        }
    }
}
