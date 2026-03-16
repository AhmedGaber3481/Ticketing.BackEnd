using LinkDev.Ticketing.API.Helpers;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Helpers;
using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.WebAPI.Controllers
{
    [Route("api/DashBoard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly ITicketService _ticketService;
        private readonly string? _currentCulture;
        private readonly ILoggedUserService _loggedUserService;

        public DashboardController(Logging.Application.Interfaces.ILogger logger,
            ITicketService ticketService,
            CultureHelper cultureHelper,
            ILoggedUserService loggedUserService)
        {
            _logger = logger;
            _ticketService = ticketService;
            _currentCulture = cultureHelper.Culture ?? "en-US";
            _loggedUserService = loggedUserService;
        }
        
        [HttpGet]
        [Route("GetStatistics")]
        public async Task<ActionResult> GetStatistics()
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                string userId = await _loggedUserService.GetLoggedUserId();

                _logger.LogInformation("GetStatistics user", "TicketingController", "GetTickets", correlationId, id1: userId);

                var tickets = _ticketService.GetTicketStatistics(_currentCulture!, userId, correlationId);

                return ResponseMessageHelper.Ok(tickets);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception", "TicketingController", "GetTickets", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
