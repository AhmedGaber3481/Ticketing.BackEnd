using LinkDev.Ticketing.API.Helpers;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Helpers;
using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.API.Controllers
{
    [Route("api/Ticketing")]
    [ApiController]
    public class TicketingController : ControllerBase
    {
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly ITicketService _ticketService;
        private readonly string? _currentCulture;
        //private readonly ILoggedUserService _loggedUserService;

        public TicketingController(Logging.Application.Interfaces.ILogger logger,
            ITicketService ticketService,
            CultureHelper cultureHelper)
        {
            _logger = logger;
            _ticketService = ticketService;
            _currentCulture = cultureHelper.Culture;
            //_loggedUserService = loggedUserService;
        }
        
        [HttpGet]
        [Route("GetTickets")]
        public ActionResult GetTickets([FromQuery]TicketRequestDTO requestDTO)
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                _logger.LogInformation("Get Tickets Page:" + requestDTO.PageNumber, "TicketingController", "GetTickets", correlationId);
                requestDTO.Culture = _currentCulture;
                var tickets = _ticketService.GetTickets(requestDTO);
                
                return ResponseMessageHelper.Ok(tickets);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception", "TicketingController", "GetTickets", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
        
        [HttpPost]
        [Route("SaveTicket")]
        public  async Task<ActionResult> SaveTicket([FromForm] Application.DTos.TicketDTO ticketDTO)
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                if (!ModelState.IsValid)
                    return ResponseMessageHelper.BadRequest(ResponseErrorMessage.GetErrorMessages(ModelState));

                _logger.LogInformation(ticketDTO, "TicketingController", "AddTicket", correlationId);

                //ticketDTO.UserId = await _loggedUserService.GetLoggedUserId();

                var response = _ticketService.SaveTicket(ticketDTO, _currentCulture!, correlationId);

                return ResponseMessageHelper.GetResult(response);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in adding Ticket", "TicketingController", "AddTicket", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }

        [HttpGet]
        [Route("GetTicket/{ticketId}")]
        public ActionResult GetTicket(int? ticketId) 
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                _logger.LogInformation("TicketId", "TicketingController", "GetTicket", correlationId, id1: ticketId?.ToString());
                var response = _ticketService.GetTicket(ticketId ?? 0, _currentCulture!, correlationId);

                return ResponseMessageHelper.GetResult(response);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in adding Ticket", "TicketingController", "AddTicket", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
