using Newtonsoft.Json;
using System.Net;

namespace LinkDev.UserManagent.Domain.Models
{
    public class ResponseMessage<T>
    {
        public ResponseMessage()
        {
            Status = (int)HttpStatusCode.OK;
        }

        public ResponseMessage(int Status)
        {
            this.Status = Status;
        }

        [JsonProperty("notifications")]
        public IEnumerable<string> Notifications { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        //public Guid CorrelationId { get; set; }

    }

}
