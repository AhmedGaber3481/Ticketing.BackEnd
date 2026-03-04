using LinkDev.Ticketing.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
