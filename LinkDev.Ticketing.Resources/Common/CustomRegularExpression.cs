using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Ticketing.Resources.Common
{
    public class CustomRegularExpression : RegularExpressionAttribute
    {
        public Type ErrorMessageResourceType { get; set; } = typeof(Messages);
        public string? ErrorMessageResourceName { get; set; }

        public CustomRegularExpression(string pattern) : base(pattern) { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Use the base RegularExpressionAttribute validation
            ValidationResult? baseResult = base.IsValid(value, validationContext);

            if (baseResult != ValidationResult.Success)
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
                        return Messages.ResourceManager.GetString(errorCode, culture) ?? "Invalid input format.";
                    }
                }
            }

            return ErrorMessage ?? "Invalid input format."; // Fallback message
        }
    }
}