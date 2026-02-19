using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketCategory
    {
        public required string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        public short LangId { get; set; }
    }
}
