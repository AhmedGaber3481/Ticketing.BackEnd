using LinkDev.Ticketing.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.Ticketing.Domain.Entities
{
    public abstract class BaseLookup
    {
        public bool IsDeleted { get; set; }

        [Required]
        public short LangId { get; set; }

        public abstract LookupDTO ToLookupDTO();
    }
}
