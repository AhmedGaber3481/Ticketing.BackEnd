using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.Ticketing.Domain.Entities
{
    public class TicketView
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        //public int TypeId { get; set; }
        //public int CategoryId { get; set; }
        //public int SubCategoryId { get; set; }
        //public int PriorityId { get; set; }
        //public int StatusId { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? TicketType { get; set; }
        public string? TicketCategory { get; set; }
        //public string? TicketSubCategory{ get; set;}
    }

    public class ScalarInt
    {
        public int Value { get; set; }
    }

}
