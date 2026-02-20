using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;

namespace LinkDev.Ticketing.Application.Interfaces
{
    public interface ILookupRepository
    {
        IEnumerable<LookupDTO>? GetLookup<T>(string lookupType, string culture) where T : BaseLookup;
    }
}
