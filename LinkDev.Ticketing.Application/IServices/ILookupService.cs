using LinkDev.Ticketing.Core.Models;

namespace LinkDev.Ticketing.Application.IServices
{
    public interface ILookupService
    {
        IEnumerable<LookupDTO>? GetLookup(string lookupType, string culture);
    }

}
