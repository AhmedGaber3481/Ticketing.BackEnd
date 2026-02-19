using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class AppLanguage
    {
        [Key]
        public short Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
