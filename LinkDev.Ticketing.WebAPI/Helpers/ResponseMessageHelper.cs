using LinkDev.Ticketing.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using LinkDev.Ticketing.Resources;

namespace LinkDev.Ticketing.API.Helpers
{
    public class ResponseMessageHelper
    {
        public static JsonResult BadRequest(IEnumerable<string> errorMessages)
        {
            ResponseMessage<object> response = new ResponseMessage<object>((int)HttpStatusCode.BadRequest)
            {
                Notifications = errorMessages
            };

            return new JsonResult(response) { StatusCode = response.Status };
        }

        public static JsonResult BadRequest<T>(ResponseMessage<T> response)
        {
            //response.Data = default;
            response.Status = (int)HttpStatusCode.BadRequest;

            return new JsonResult(response) { StatusCode = response.Status };
        }

        public JsonResult BadRequestResult()
        {
            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.BadRequest };
        }

        public static JsonResult ServerError(Guid correlationId)
        {
            ServerErrorMessage errorMessage = new ServerErrorMessage()
            {
                TraceId = correlationId.ToString(),
                Message = Messages.InternalServerError,
                Status = (int)HttpStatusCode.InternalServerError
            };

            return new JsonResult(errorMessage) { StatusCode = errorMessage.Status };
        }

        public static JsonResult Ok<T>(ResponseMessage<T> response)
        {
            response.Status = (int)HttpStatusCode.OK;

            return new JsonResult(response) { StatusCode = response.Status };
        }

        public static JsonResult GetResult<T>(ResponseMessage<T> response)
        {
            if (response.Status == 0)
            {
                response.Status = (int)HttpStatusCode.OK;
            }
            return new JsonResult(response) { StatusCode = response.Status };
        }

        public static JsonResult Ok<T>(T data, IEnumerable<string> notifications = null)
        {
            ResponseMessage<T> response = new ResponseMessage<T>((int)HttpStatusCode.OK)
            {
                Data = data,
                Notifications = notifications
            };

            return new JsonResult(response) { StatusCode = response.Status };
        }

        public static JsonResult Forbid(string[] errorMessages)
        {
            int status = (int)HttpStatusCode.Forbidden;
            ResponseMessage<object> response = new ResponseMessage<object>(status)
            {
                Notifications = errorMessages
            };

            return new JsonResult(response) { StatusCode = status };
        }
        public static string GetValidationMessages<T>(ResponseMessage<T> response)
        {
            if (response.Notifications != null && response.Notifications.Any())
            {
                StringBuilder msgBuilder = new StringBuilder();

                foreach (string notification in response.Notifications)
                {
                    msgBuilder.AppendLine(notification);
                }
                return msgBuilder.ToString();
            }
            return string.Empty;
        }

    }
}
