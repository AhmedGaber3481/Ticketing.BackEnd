namespace LinkDev.Ticketing.Core.Models
{
    public class ServerErrorMessage
    {
        public string Message { get; set; }

        public string TraceId { get; set; }

        public int Status { get; set; }
    }
}
