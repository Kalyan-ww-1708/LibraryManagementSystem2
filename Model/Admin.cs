namespace LibraryManagementSystem.Model
{
    public class Admin
    {
        public Guid AdminId { get; set; }
        public required string AdminName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
    }
}
