using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Globalization;

namespace LinkDev.Ticketing.Resources.Common
{
    public class RequiredExtension : RequiredAttribute
    {
        public Type ErrorMessageResourceType { get; set; } = typeof(Messages);
        public string? ErrorMessageResourceName { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                string errorMessage = GetErrorMessage(CultureInfo.CurrentCulture); // Uses current culture
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(CultureInfo culture)
        {
            if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                // Use reflection to get the static property value from the resource class
                PropertyInfo? resourceProperty = ErrorMessageResourceType.GetProperty(
                    ErrorMessageResourceName,
                    BindingFlags.Static | BindingFlags.Public
                );

                if (resourceProperty != null)
                {
                    object? value = resourceProperty.GetValue(null);
                    if (value is string errorCode)
                    {
                        // If it's a valid error code, fetch the localized message
                        return Messages.ResourceManager.GetString(errorCode, culture) ?? "This field is required.";
                    }
                }
            }

            return ErrorMessage ?? "This field is required."; // Fallback message
        }
    }
}