namespace LinkDev.Ticketing.Application.Dtos
{
    public class TicketSearchResult<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T>? Items { get; set; }
    }
}
