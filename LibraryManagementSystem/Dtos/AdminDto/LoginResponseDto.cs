namespace LibraryManagementSystem.Dtos.AdminDto
{
    public class LoginResponseDto
    {
        public required string Email { get; set; }
        public required string AdminName { get; set; }
        public required string Token { get; set; }
        public DateTime OtpExpiry { get; set; }
    }
}
