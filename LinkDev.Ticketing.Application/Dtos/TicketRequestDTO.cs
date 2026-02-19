namespace LinkDev.Ticketing.Application.Dtos
{
    public class TicketRequestDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchValue { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }

    }
}
