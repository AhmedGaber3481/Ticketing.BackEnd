using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int? Type { get; set; }
        public int? Category { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }

        [MaxLength(256)]
        public string? CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [MaxLength(256)]
        public string? ModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        public TicketCategory? TicketCategory { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public TicketStatus? TicketStatus { get; set; }
        //public TicketSubCategory? TicketSubCategory { get; set; }
        public TicketType? TicketType { get; set; }

        //public ICollection<TicketTransaction>? TicketTransactions { get; set; }

        public ICollection<TicketAttachment>? TicketAttachments { get; set; }
    }

}
