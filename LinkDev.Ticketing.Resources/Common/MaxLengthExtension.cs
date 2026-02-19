using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace LinkDev.Ticketing.Resources.Common
{
    public class MaxLengthExtension : MaxLengthAttribute
    {
        public Type ErrorMessageResourceType { get; set; } = typeof(Messages);
        public string? ErrorMessageResourceName { get; set; }

        public MaxLengthExtension(int length) : base(length) { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && str.Length > Length)
            {
                string errorMessage = GetErrorMessage(CultureInfo.CurrentCulture);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(CultureInfo culture)
        {
            if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                PropertyInfo? resourceProperty = ErrorMessageResourceType.GetProperty(
                    ErrorMessageResourceName,
                    BindingFlags.Static | BindingFlags.Public
                );

                if (resourceProperty != null)
                {
                    object? value = resourceProperty.GetValue(null);
                    if (value is string errorCode)
                    {
                        return Messages.ResourceManager.GetString(errorCode, culture) ?? $"Maximum length is {Length} characters.";
                    }
                }
            }

            return ErrorMessage ?? $"Maximum length is {Length} characters."; // Fallback message
        }
    }
}
