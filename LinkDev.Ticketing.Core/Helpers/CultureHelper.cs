using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace LinkDev.Ticketing.Core.Helpers
{
    public class CultureHelper
    {
        public string? Culture { get; set; }

        public CultureHelper(IHttpContextAccessor httpContextAccessor)
        {
            if(httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Accept-Language", out var culture))
            {
               Culture = culture.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0];
            }
        }
    }
}
