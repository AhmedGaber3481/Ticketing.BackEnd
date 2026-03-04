using LinkDev.Ticketing.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class BaseLookup
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public short LangId { get; set; }

        public required string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public LookupDTO ToLookupDTO()
        {
            return new LookupDTO
            {
                Id = this.Id,
                Code = this.Code,
                Name = this.Name,
                LangId = this.LangId
            };
        }
    }
}
