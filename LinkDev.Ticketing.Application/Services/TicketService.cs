using LinkDev.Ticketing.Application.IServices;
using LinkDev.Ticketing.Application.DTos;
using LinkDev.Ticketing.Application.Interfaces;
using Mapster;
using LinkDev.Ticketing.Application.Dtos;
using LinkDev.Ticketing.Core.Models;
using LinkDev.Ticketing.Domain.Entities;
using System.Net;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using LinkDev.Ticketing.Core.Helpers;

namespace LinkDev.Ticketing.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        //private readonly IRepository<Domain.Entities.TicketCategory> _categoryRepository;
        //private readonly IRepository<Domain.Entities.TicketPriority> _priorityRepository;
        //private readonly IRepository<Domain.Entities.TicketStatus> _statusRepository;
        //private readonly IRepository<Domain.Entities.TicketSubCategory> _subCategoryRepository;
        //private readonly IRepository<Domain.Entities.TicketType> _typeRepository;
        private readonly IRepository<Domain.Entities.TicketAttachment> _attachmentRepository;
        private readonly Logging.Application.Interfaces.ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileManager _fileManager;

        public TicketService(
            ITicketRepository ticketRepository,
            //IRepository<Domain.Entities.TicketCategory> categoryRepository,
            //IRepository<Domain.Entities.TicketPriority> priorityRepository,
            //IRepository<Domain.Entities.TicketStatus> statusRepository,
            //IRepository<Domain.Entities.TicketSubCategory> subCategoryRepository,
            //IRepository<Domain.Entities.TicketType> typeRepository,
            IRepository<Domain.Entities.TicketAttachment> attachmentRepository,
            Logging.Application.Interfaces.ILogger logger,
            IUnitOfWork unitOfWork,
            FileManager fileManager)
        {
            _ticketRepository = ticketRepository;
            //_categoryRepository = categoryRepository;
            //_priorityRepository = priorityRepository;
            //_statusRepository = statusRepository;
            //_subCategoryRepository = subCategoryRepository;
            //_typeRepository = typeRepository;
            _attachmentRepository = attachmentRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
        }

        public TicketSearchResult<TicketView> GetTickets(TicketRequestDTO requestDTO)
        {
            _logger.LogInformation("GetTickets", "TicketService", "GetTickets", Guid.NewGuid());

            var tickets = _ticketRepository.GetTicketViews(1);
            var totalcount = tickets.Count();//GetAll().Count();

            // Pre-fetch related entities for name mapping
            //var categories = _categoryRepository.GetAll().ToList();
            //var priorities = _priorityRepository.GetAll().ToList();
            //var statuses = _statusRepository.GetAll().ToList();
            //var subCategories = _subCategoryRepository.GetAll().ToList();
            //var types = _typeRepository.GetAll().ToList();

            // Query tickets, apply search if needed
            //var ticketQuery = _ticketRepository.GetAll();
            //if (!string.IsNullOrEmpty(requestDTO.SearchValue))
            //{
            //    ticketQuery = ticketQuery.Where(x => x.Title.Contains(requestDTO.SearchValue) || x.Description.Contains(requestDTO.SearchValue));
            //}

            var ticketList = tickets
                .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
                .Take(requestDTO.PageSize)
                .ToList();

            //var list = pagedTickets.Select(x =>
            //{
            //    var dto = x.Adapt<TicketDTO>();
            //    dto.Category = x.CategoryId.HasValue ? categories.FirstOrDefault(c => c.CategoryId == x.CategoryId)?.Name : null;
            //    dto.Priority = x.PriorityId.HasValue ? priorities.FirstOrDefault(p => p.PriorityId == x.PriorityId)?.Name : null;
            //    dto.Status = x.StatusId.HasValue ? statuses.FirstOrDefault(s => s.StatusId == x.StatusId)?.Name : null;
            //    dto.SubCategory = x.SubCategoryId.HasValue ? subCategories.FirstOrDefault(sc => sc.SubCategoryId == x.SubCategoryId)?.Name : null;
            //    dto.Type = x.TypeId.HasValue ? types.FirstOrDefault(t => t.TypeId == x.TypeId)?.Name : null;
            //    return dto;
            //}).ToList();

            return new TicketSearchResult<TicketView>
            {
                PageNumber = requestDTO.PageNumber,
                PageSize = requestDTO.PageSize,
                Items = ticketList,
                TotalCount = totalcount
            };
        }

        public ResponseMessage<bool> SaveTicket(TicketDTO ticketDTO, Guid correlationId)
        {
            ResponseMessage<bool> response = null;
            IDbContextTransaction? transaction = null;
            try
            {
                _logger.LogInformation("Save Ticket before validate: " + ticketDTO.Title, "TicketingService", "AddTicket", correlationId);

                //response = ValidateTicket(ticketDTO);

                //if (!response.Data)
                //{
                //    return response;
                //}

                transaction = _unitOfWork.BeginTransaction();
                Ticket? ticket = _ticketRepository.GetById(ticketDTO.Id);
                if (ticket != null)
                {
                    ticket = MapTicket(ticketDTO, ticket);
                    ticket.CreatedAt = DateTime.Now;
                    _ticketRepository.Update(ticket);
                }
                else
                {
                    ticket = MapTicket(ticketDTO);
                    ticket.CreatedAt = DateTime.Now;
                    _ticketRepository.Add(ticket);
                }


                _unitOfWork.SaveChanges();

                SaveAttachments(ticketDTO.Files, ticket.Id);

                _unitOfWork.SaveChanges();
                transaction.Commit();

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

        //private ResponseMessage<bool> ValidateTicket(TicketDTO ticketDTO)
        //{
        //    ResponseMessage<bool> response = new ResponseMessage<bool>();
        //    response.Data = true;

        //    // Validate related entities exist
        //    if (ticketDTO.CategoryId.HasValue && !_categoryRepository.GetAll().Any(c => c.CategoryId == ticketDTO.CategoryId))
        //    {
        //        response.Status = (int)HttpStatusCode.BadRequest;
        //        response.Notifications = ["Invalid Category ID"];
        //        response.Data = false;
        //    }
        //    else if (ticketDTO.PriorityId.HasValue && !_priorityRepository.GetAll().Any(p => p.PriorityId == ticketDTO.PriorityId))
        //    {
        //        response.Status = (int)HttpStatusCode.BadRequest;
        //        response.Notifications = ["Invalid Priority ID"];
        //        response.Data = false;
        //    }
        //    else if (ticketDTO.StatusId.HasValue && !_statusRepository.GetAll().Any(s => s.StatusId == ticketDTO.StatusId))
        //    {
        //        response.Status = (int)HttpStatusCode.BadRequest;
        //        response.Notifications = ["Invalid Status ID"];
        //        response.Data = false;
        //    }
        //    else if (ticketDTO.SubCategoryId.HasValue && !_subCategoryRepository.GetAll().Any(sc => sc.SubCategoryId == ticketDTO.SubCategoryId))
        //    {
        //        response.Status = (int)HttpStatusCode.BadRequest;
        //        response.Notifications = ["Invalid SubCategory ID"];
        //        response.Data = false;
        //    }
        //    else if (ticketDTO.TypeId.HasValue && !_typeRepository.GetAll().Any(t => t.TypeId == ticketDTO.TypeId))
        //    {
        //        response.Status = (int)HttpStatusCode.BadRequest;
        //        response.Notifications = ["Invalid Type ID"];
        //        response.Data = false;
        //    }

        //    return response;
        //}

        private Ticket MapTicket(TicketDTO source, Ticket? destination)
        {
            Ticket ticket = destination == null ? new Ticket() { Title = string.Empty} : destination;
            ticket.Title = source.Title ?? string.Empty;
            ticket.Description = source.Description;
            //ticket.CategoryId = source.CategoryId;
            //ticket.PriorityId = source.PriorityId;
            //ticket.StatusId = source.StatusId;
            //ticket.SubCategoryId = source.SubCategoryId;
            //ticket.TypeId = source.TypeId;
            return ticket;
        }

        private Ticket MapTicket(TicketDTO source)
        {
            return MapTicket(source, null);
        }

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
