namespace LinkDev.UserManagent.Infrastructure.Entities
{
    public class UserDetails
    {
        public required string UserId { get; set; }
        public string? FullName { get; set; }
        public required AspNetUsers User { get; set; }
    }
}
