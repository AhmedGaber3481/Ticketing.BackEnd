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
        public string? TicketType { get; set; }
        public string? TicketStatus { get; set; }
        public string? TicketPriority { get; set; }
        public string? TicketCategory { get; set; }
    }
}
