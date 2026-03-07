using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;

namespace LinkDev.Ticketing.Application.Interfaces
{
    public interface ILookupRepository
    {
        List<LookupDTO>? GetLookup<T>(string lookupType, string culture) where T : BaseLookup;
        int? GetLookupItemId<T>(LookupType lookupType, string itemCode, string culture) where T : BaseLookup;
    }
}
