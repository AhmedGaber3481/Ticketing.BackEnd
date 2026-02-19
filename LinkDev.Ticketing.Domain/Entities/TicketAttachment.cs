using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        [Required]
        public int TicketId { get; set; }
        [MaxLength(100)]
        public string? AttachmentName { get; set; }
        [MaxLength(256)]
        public string? AttachmentUrl { get; set; }
    }
}
