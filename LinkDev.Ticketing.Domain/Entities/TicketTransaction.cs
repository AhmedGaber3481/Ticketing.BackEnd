using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketTransaction
    {
        public long Id { get; set; }
        [Required]
        public int TicketId { get; set; }
        public short? TypeId { get; set; }
        public short? StatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        [MaxLength(256)]
        public string? CreatedBy { get; set; }
        public Ticket? Ticket { get; set; }
        public TicketTransactionStatus? TicketTransactionStatus { get; set; }
        public TicketTransactionType? TicketTransactionType { get; set; }
    }
}
