using LibraryManagementSystem.Services.Interfaces;

namespace LibraryManagementSystem.Services
{
    public class OtpService: IOtpService
    {
        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
