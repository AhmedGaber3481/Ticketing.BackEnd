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
        private readonly LookupFactory _lookupFactory;

        public LookupController(Logging.Application.Interfaces.ILogger logger
            , CultureHelper cultureHelper
            , LookupFactory lookupFactory)
        {
            _logger = logger;
            _currentCulture = cultureHelper.Culture ?? "en-US";
            _lookupFactory = lookupFactory;
        }
        
        [HttpGet("GetLookup")]
        public IActionResult GetLookup(string lookupType)
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                var lookupService = _lookupFactory.GetInstance(lookupType, _currentCulture);
                var lookupItems = lookupService.GetLookup(lookupType, _currentCulture);

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
