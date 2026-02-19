using LinkDev.Ticketing.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LinkDev.Ticketing.Resources.Common
{
    public class EmailValidation : CustomRegularExpression
    {
        public Type ErrorMessageResourceType { get; set; } = typeof(Messages);
        public string? ErrorMessageResourceName { get; set; }

        public EmailValidation() : base(Constants.EmailRegex)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Allow empty values if required validation is separate
            }

            if (!IsValidPattern(Pattern))
            {
                return new ValidationResult("Invalid regex pattern configuration.");
            }

            string input = value?.ToString() ?? string.Empty;
            if (!Regex.IsMatch(input, Pattern))
            {
                string errorMessage = GetErrorMessage(CultureInfo.CurrentCulture);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
        private bool IsValidPattern(string pattern)
        {
            try
            {
                _ = new Regex(pattern);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
                        return Messages.ResourceManager.GetString(errorCode, culture) ?? "Invalid input format.";
                    }
                }
            }

            return ErrorMessage ?? "Invalid input format."; // Fallback message
        }
    }
}