using LinkDev.Ticketing.Core.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinkDev.Ticketing.Core.Models
{
    public class ResponseErrorMessage
    {
        public ErrorCodes? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public static List<string> GetErrorMessages(ModelStateDictionary modelState)
        {
            List<string> errorMessages = new List<string>();

            foreach (string key in modelState.Keys)
            {
                var errors = modelState[key]?.Errors;
                if (errors != null && errors.Count() > 0)
                {
                    errorMessages.Add(errors[0].ErrorMessage);
                }
            }
            return errorMessages;
        }
    }
}
