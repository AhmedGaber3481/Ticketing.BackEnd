using LinkDev.Ticketing.API.Helpers;
using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Helpers;
using LinkDev.Ticketing.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.API.Controllers
{
    [Route("Ticketing")]
    [ApiController]
    public class TicketingController : ControllerBase
    {
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly ITicketService _ticketService;
        private readonly string? _currentCulture;

        public TicketingController(Logging.Application.Interfaces.ILogger logger, ITicketService ticketService, CultureHelper cultureHelper)
        {
            _logger = logger;
            _ticketService = ticketService;
            _currentCulture = cultureHelper.Culture;
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
        public  ActionResult SaveTicket([FromForm] Application.DTos.TicketDTO ticketDTO)
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                if (!ModelState.IsValid)
                    return ResponseMessageHelper.BadRequest(ResponseErrorMessage.GetErrorMessages(ModelState));

                _logger.LogInformation("Add Ticket with Title: " + ticketDTO.Title, "TicketingController", "AddTicket", correlationId);
                var response = _ticketService.SaveTicket(ticketDTO, correlationId);

                return ResponseMessageHelper.Ok(response);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in adding Ticket", "TicketingController", "AddTicket", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
