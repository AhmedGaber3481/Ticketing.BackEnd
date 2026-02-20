using LinkDev.Ticketing.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketCategory : BaseLookup
    {
        public required string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public override LookupDTO ToLookupDTO()
        {
            return new LookupDTO
            {
                Code = this.Code,
                Name = this.Name,
                LangId = this.LangId
            };
        }
    }
}
