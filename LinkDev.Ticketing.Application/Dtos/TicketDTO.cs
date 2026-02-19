using System.ComponentModel.DataAnnotations;
using LinkDev.Ticketing.Resources;
using LinkDev.Ticketing.Resources.Common;
using Microsoft.AspNetCore.Http;

namespace LinkDev.Ticketing.Application.DTos
{
    public class TicketDTO
    {
        public int Id { get; set; }

        [RequiredExtension(ErrorMessageResourceName = "RequiredField")]
        public string? Title { get; set; }

        [RequiredExtension(ErrorMessageResourceName = "RequiredField")]
        [MaxLengthExtension(1000, ErrorMessageResourceName = "MaxLengthField")]
        public string? Description { get; set; }

        [RequiredExtension(ErrorMessageResourceName = "RequiredField")]
        [DataTypeExtension(DataType.PhoneNumber, ErrorMessageResourceName = "PhoneNumberField")]
        public string PhoneNumber { get; set; }

        [RequiredExtension(ErrorMessageResourceName = "RequiredField")]
        [EmailValidation(ErrorMessageResourceName = "EmailValidationField")]
        [MaxLengthExtension(100, ErrorMessageResourceName = "MaxLengthField")]
        public string Email { get; set; }

        public int? CategoryId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? TypeId { get; set; }

        public string? Category { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public string? SubCategory { get; set; }
        public string? Type { get; set; }

        //[AllowedExtensions([".jpg", ".jepg", ".png", ".pdf"], ErrorMessageResourceName = "InvalidFileExtension")]
        //[MaxFileSizeExtention(15, ErrorMessageResourceName = "MaxFileSizeError")]
        public IFormFile[]? Files { get; set; }
    }
}
