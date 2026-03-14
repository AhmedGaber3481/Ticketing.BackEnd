using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Domain.DTOs
{
    public class UserSearchDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchValue { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
