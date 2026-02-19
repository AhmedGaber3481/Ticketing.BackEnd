using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketTransactionType
    {
        public short TypeId { get; set; }
        [Required]
        public short Code { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [Required]
        public short LangId { get; set; }
        public ICollection<TicketTransaction>? TicketTransactions { get; set; }
    }
}
