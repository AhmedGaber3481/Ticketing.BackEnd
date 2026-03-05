namespace LinkDev.Ticketing.Core.Models
{
    public class TicketRequestDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchValue { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public string? Culture { get; set; }
        public int? TicketType { get; set; }
        public int? TicketStatus { get; set; }
        public int? TicketPriority { get; set; }
        public int? TicketCategory { get; set; }
    }
}
