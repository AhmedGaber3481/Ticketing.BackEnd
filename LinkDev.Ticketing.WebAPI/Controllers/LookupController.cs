using LinkDev.Ticketing.API.Helpers;
using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Application.Services;
using LinkDev.Ticketing.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.WebAPI.Controllers
{
    [Route("Ticketing/api/Lookup")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly string _currentCulture;
        private readonly ILookupService _lookupService;

        public LookupController(Logging.Application.Interfaces.ILogger logger
            , CultureHelper cultureHelper
            , ILookupService lookupService)
        {
            _logger = logger;
            _currentCulture = cultureHelper.Culture ?? "en-US";
            _lookupService = lookupService;
        }
        
        [HttpGet("GetLookup")]
        public IActionResult GetLookup(string lookupType)
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                var lookupItems = _lookupService.GetLookup(lookupType, _currentCulture);

                return ResponseMessageHelper.Ok(lookupItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ticket lookups.", "LookupController", "GetLookup", correlationId, id1: lookupType);

                return ResponseMessageHelper.ServerError(correlationId);
            }

        }
    }
}
