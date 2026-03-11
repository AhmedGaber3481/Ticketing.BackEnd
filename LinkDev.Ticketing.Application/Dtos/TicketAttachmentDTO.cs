using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Application.Dtos
{
    public class TicketAttachmentDTO
    {
        public int AttachmentId { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
