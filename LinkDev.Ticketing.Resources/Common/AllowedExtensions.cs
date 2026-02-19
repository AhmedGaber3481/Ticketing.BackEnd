using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace LinkDev.Ticketing.Resources.Common
{
    public class AllowedExtensions : ValidationAttribute
    {
        public string[] AllowedExtensionsArr { get; }
        public Type ErrorMessageResourceType { get; set; } = typeof(Messages);
        public string? ErrorMessageResourceName { get; set; }

        public AllowedExtensions(string[] allowedExtensions)
        {
            AllowedExtensionsArr = allowedExtensions.Select(e => e.StartsWith(".") ? e.ToLower() : $".{e.ToLower()}").ToArray();
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string fileName) // Case when only file name is provided
            {
                return ValidateFileExtension(fileName);
            }

            if (value is IFormFile file) // Case for uploaded files in ASP.NET Core
            {
                return ValidateFileExtension(file.FileName);
            }

            return ValidationResult.Success; // If no file, no validation error
        }

        private ValidationResult? ValidateFileExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (!AllowedExtensionsArr.Contains(extension))
            {
                string errorMessage = GetErrorMessage(CultureInfo.CurrentCulture, extension);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(CultureInfo culture, string invalidExtension)
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
                        return string.Format(localizedMessage ?? "Invalid file extension: {0}.", invalidExtension);
                    }
                }
            }

            return ErrorMessage ?? $"Invalid file extension: {invalidExtension}."; // Fallback message
        }
    }
}