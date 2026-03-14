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

        public ListViewResult<TicketView> GetTickets(TicketRequestDTO requestDTO, Guid correlationId, string userId)
        {
            _logger.LogInformation("GetTickets", "TicketService", "GetTickets", correlationId);
            
            var tickets = _ticketRepository.GetTickets(requestDTO, userId, correlationId, out int totalCount);

            return new ListViewResult<TicketView>
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
                Ticket? ticket = _ticketRepository.FirstOrDefault(x => x.Id == ticketDTO.Id, x => x.TicketAttachments!);
                if (ticket != null)
                {
                    MapTicket(ticketDTO, ref ticket, culture);
                    _ticketRepository.Update(ticket);
                }
                else
                {
                    MapTicket(ticketDTO, ref ticket!, culture);
                    ticket.CreatedAt = ticketDTO.CreatedAt.HasValue ? ticketDTO.CreatedAt.Value : DateTime.Now;
                    _ticketRepository.Add(ticket);
                }

                _unitOfWork.SaveChanges();

                SaveAttachments(ticketDTO.Files, ticket.Id, ticket, ticketDTO.AttachmentSerials);

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

        public ResponseMessage<TicketDTO> GetTicket(int ticketId, string culture, Guid correlationId)
        {
            try
            {
                var ticket = _ticketRepository.FirstOrDefault(x=> x.Id == ticketId, x => x.TicketAttachments!);
                if(ticket == null)
                {
                    return new ResponseMessage<TicketDTO>() { Status = (int)HttpStatusCode.NotFound};
                }
                var ticketDTO = MapTicket(ticket, culture);

                return new ResponseMessage<TicketDTO>() { Data = ticketDTO };
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in adding the ticket", "TicketingService", "GetTicket", correlationId);

                return new ResponseMessage<TicketDTO>() { Status =  (int)HttpStatusCode.InternalServerError };
            }
        }

        private void MapTicket(TicketDTO source, ref Ticket destination, string culture)
        {
            if(destination == null)
            {
                destination = new Ticket() { Title = string.Empty };
                destination.CreatedBy = source.UserId;
            }
            else
            {
                destination.ModifiedBy = source.UserId;
                destination.LastModifiedAt = DateTime.Now;
            }
            destination.Title = source.Title ?? string.Empty;
            destination.Description = source.Description;
            if (!string.IsNullOrEmpty(source.Category))
            {
                destination.Category = _lookupRepository.GetLookupItemId<TicketCategoryLookup>(LookupType.TicketCategory, source.Category, culture);
            }
            if (!string.IsNullOrEmpty(source.Type))
            {
                destination.Type = _lookupRepository.GetLookupItemId<TicketTypeLookup>(LookupType.TicketType, source.Type, culture);
            }
            if (!string.IsNullOrEmpty(source.Priority))
            {
                destination.Priority = _lookupRepository.GetLookupItemId<TicketPriorityLookup>(LookupType.TicketPriority, source.Priority, culture);
            }
            if (!string.IsNullOrEmpty(source.Status))
            {
                destination.Status = _lookupRepository.GetLookupItemId<TicketStatusLookup>(LookupType.TicketStatus, source.Status, culture);
            }
        }

        private TicketDTO MapTicket(Ticket ticket, string culture)
        {
            TicketDTO ticketDTO = new TicketDTO()
            {
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                Id = ticket.Id
            };
            if (ticket.Category > 0)
            {
                ticketDTO.Category = _lookupRepository.GetLookupItemCode<TicketCategoryLookup>(LookupType.TicketCategory, ticket.Category, culture);
            }
            if (ticket.Priority > 0)
            {
                ticketDTO.Priority = _lookupRepository.GetLookupItemCode<TicketPriorityLookup>(LookupType.TicketPriority, ticket.Priority, culture);
            }
            if (ticket.Type > 0)
            {
                ticketDTO.Type = _lookupRepository.GetLookupItemCode<TicketTypeLookup>(LookupType.TicketType, ticket.Type, culture);
            }
            if (ticket.Status > 0)
            {
                ticketDTO.Status = _lookupRepository.GetLookupItemCode<TicketStatusLookup>(LookupType.TicketStatus, ticket.Status, culture);
            }
            if (ticket.TicketAttachments != null)
            {
                ticketDTO.Attachments = ticket.TicketAttachments.Select(x => new TicketAttachmentDTO()
                {
                    AttachmentId = x.Id,
                    AttachmentName = x.AttachmentName,
                    AttachmentUrl = x.AttachmentUrl
                }).ToArray();
            }

            return ticketDTO;
        }
        
        private void SaveAttachments(IFormFile[]? attachments, int ticketId, Ticket ticket, string[]? AttachmentSerials)
        {
            //delete old attachmnts
            if (ticket != null && ticket.TicketAttachments != null)
            {
                List<TicketAttachment> deletedAttachments = new List<TicketAttachment>();
                if (AttachmentSerials != null && AttachmentSerials.Length > 0)
                {
                    // iterate on ticket attachments in database
                    foreach (var item in ticket.TicketAttachments)
                    {
                        if (!AttachmentSerials.Contains(item.Id.ToString()))
                        {
                            //delete attachment that exist in database but not in given list of ticket attachments
                            deletedAttachments.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in ticket.TicketAttachments)
                    {
                        //delete attachment
                        deletedAttachments.Add(item);
                    }
                }

                if (deletedAttachments.Count > 0)
                {
                    _attachmentRepository.DeleteRange(deletedAttachments.ToArray());
                }
            }

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
