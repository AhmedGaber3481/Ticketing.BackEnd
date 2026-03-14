using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Core.Models
{
    public class ListViewResult<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}
