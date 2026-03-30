using System.Globalization;

namespace LibraryManagementSystem.Model
{
    public class User
    {
        public Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
