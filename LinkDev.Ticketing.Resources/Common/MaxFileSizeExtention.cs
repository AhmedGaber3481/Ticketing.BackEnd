using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection;

namespace LinkDev.Ticketing.Resources.Common
{
    public class MaxFileSizeExtention : ValidationAttribute
    {
        public long MaxSizeInBytes { get; }
        public Type ErrorMessageResourceType { get; set; } = typeof(Messages);
        public string? ErrorMessageResourceName { get; set; }

        public MaxFileSizeExtention(long maxSizeInMB)
        {
            MaxSizeInBytes = maxSizeInMB * 1024 * 1024; // Convert MB to bytes
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > MaxSizeInBytes)
                {
                    string errorMessage = GetErrorMessage(CultureInfo.CurrentCulture, file.Length);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(CultureInfo culture, long fileSize)
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
                        string localizedMessage = Messages.ResourceManager.GetString(errorCode, culture);
                        return string.Format(localizedMessage ?? "File size must not exceed {0} MB.", MaxSizeInBytes / (1024 * 1024));
                    }
                }
            }

            return ErrorMessage ?? $"File size must not exceed {MaxSizeInBytes / (1024 * 1024)} MB."; // Fallback message
        }
    }
}