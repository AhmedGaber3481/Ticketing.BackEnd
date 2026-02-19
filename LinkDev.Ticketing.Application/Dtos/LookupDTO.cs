using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Ticketing.Application.Dtos
{
    public class LookupDTO
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public short LangId { get; set; }
    }
}
