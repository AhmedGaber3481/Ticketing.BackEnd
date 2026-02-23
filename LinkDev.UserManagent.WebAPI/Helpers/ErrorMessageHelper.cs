using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace LinkDev.UserManagent.WebAPI.Helpers
{
    public class ErrorMessageHelper
    {
        public static string GetValidationErrorMessage(ModelStateDictionary modelState)
        {
            StringBuilder errorBuilder = new StringBuilder();

            foreach (string key in modelState.Keys)
            {
                var errors = modelState[key]?.Errors;
                if (errors != null && errors.Count() > 0)
                {
                    errorBuilder.AppendLine(errors[0].ErrorMessage);
                }
            }
            return errorBuilder.ToString();
        }

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
