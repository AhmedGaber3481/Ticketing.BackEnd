using LinkDev.Ticketing.API.Helpers;
using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.WebAPI.Controllers
{
    [Route("Ticketing/api/Lookup")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public LookupController(Logging.Application.Interfaces.ILogger logger
            , IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        
        [HttpGet("GetLookup")]
        public IActionResult GetLookup(string lookupType)
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                var lookupService = new LookupFactory(_serviceProvider).GetInstance(lookupType);

                var types = lookupService.GetLookup(lookupType);
                return ResponseMessageHelper.Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ticket types.", "LookupController", "GetLookup", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }

        }
    }
}
