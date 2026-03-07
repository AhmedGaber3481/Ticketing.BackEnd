using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Application.DTos;
using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Core.Helpers;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using LinkDev.Ticketing.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using System.Text.RegularExpressions;

namespace LinkDev.Ticketing.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<TicketAttachment> _attachmentRepository;
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileManager _fileManager;
        private readonly ILookupRepository _lookupRepository;

        public TicketService(
            ITicketRepository ticketRepository,
            IRepository<TicketAttachment> attachmentRepository,
            Logging.Application.Interfaces.ILogger logger,
            IUnitOfWork unitOfWork,
            FileManager fileManager,
            ILookupRepository lookupRepository)
        {
            _ticketRepository = ticketRepository;
            _attachmentRepository = attachmentRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _lookupRepository = lookupRepository;
        }

        public TicketSearchResult<TicketView> GetTickets(TicketRequestDTO requestDTO)
        {
            _logger.LogInformation("GetTickets", "TicketService", "GetTickets", Guid.NewGuid());
            
            var tickets = _ticketRepository.GetTickets(requestDTO, out int totalCount);

            return new TicketSearchResult<TicketView>
            {
                PageNumber = requestDTO.PageNumber,
                PageSize = requestDTO.PageSize,
                Items = tickets,
                TotalCount = totalCount
            };
        }

        public ResponseMessage<bool> SaveTicket(TicketDTO ticketDTO, string culture,  Guid correlationId)
        {
            ResponseMessage<bool>? response = null;
            IDbContextTransaction? transaction = null;
            try
            {
                _logger.LogInformation("Save Ticket before validate: " + ticketDTO.Title, "TicketingService", "AddTicket", correlationId);

                transaction = _unitOfWork.BeginTransaction();
                Ticket? ticket = _ticketRepository.GetById(ticketDTO.Id);
                //if (ticket != null)
                //{
                //    ticket = MapTicket(ticketDTO, ticket, culture);
                //    ticket.CreatedAt = DateTime.Now;
                //    _ticketRepository.Update(ticket);
                //}
                //else
                //{
                    ticket = MapTicket(ticketDTO, culture);
                    ticket.CreatedAt = DateTime.Now;
                    _ticketRepository.Add(ticket);
                //}

                _unitOfWork.SaveChanges();

                SaveAttachments(ticketDTO.Files, ticket.Id);

                _unitOfWork.SaveChanges();
                transaction.Commit();

                response = new ResponseMessage<bool>();
                response.Status = (int)HttpStatusCode.OK;
                response.Data = true;
                _logger.LogInformation("The ticket is added successfully with title : " + ticket.Title, "TicketingService", "AddTicket", correlationId);
            }
            catch (Exception exp)
            {
                if(response == null)
                {
                    response = new ResponseMessage<bool>();
                }
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Data = false;

                if (transaction != null)
                {
                   transaction.Rollback();
                }
                _logger.LogError(exp, "Exception in adding the ticket", "TicketingService", "AddTicket", correlationId);                
            }
            return response;
        }

        private Ticket MapTicket(TicketDTO source, string culture)
        {
            //Ticket ticket = destination == null ? new Ticket() { Title = string.Empty} : destination;
            Ticket ticket = new Ticket() { Title = source.Title ?? string.Empty };
            ticket.Description = source.Description;
            if (!string.IsNullOrEmpty(source.Category))
            {
                ticket.Category = _lookupRepository.GetLookupItemId<TicketCategoryLookup>(LookupType.TicketCategory, source.Category, culture);
            }
            if (!string.IsNullOrEmpty(source.Type))
            {
                ticket.Type = _lookupRepository.GetLookupItemId<TicketTypeLookup>(LookupType.TicketType, source.Type, culture);
            }
            if (!string.IsNullOrEmpty(source.Priority))
            {
                ticket.Priority = _lookupRepository.GetLookupItemId<TicketPriorityLookup>(LookupType.TicketPriority, source.Priority, culture);
            }
            if (!string.IsNullOrEmpty(source.Status))
            {
                ticket.Status = _lookupRepository.GetLookupItemId<TicketStatusLookup>(LookupType.TicketStatus, source.Status, culture);
            }

            return ticket;
        }

        //private Ticket MapTicket(TicketDTO source, string culture)
        //{
        //    return MapTicket(source, null, culture);
        //}

        private void SaveAttachments(IFormFile[]? attachments, int ticketId)
        {
            if (attachments != null && attachments.Length > 0)
            {
                foreach (var attachment in attachments)
                {
                    using var stream = new MemoryStream();
                    attachment.CopyTo(stream);
                    var fileBytes = stream.ToArray();

                    if (fileBytes != null && fileBytes.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
                        _fileManager.WriteFile(ticketId, fileName, fileBytes, out string fileUrl);

                        TicketAttachment ticketAttachment = new TicketAttachment()
                        {
                            AttachmentName = fileName,
                            AttachmentUrl = fileUrl,
                            TicketId = ticketId
                        };
                        _attachmentRepository.Add(ticketAttachment);
                    }
                }
            }
        }

    }
}
