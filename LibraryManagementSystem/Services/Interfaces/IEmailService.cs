namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpToMail(string email,string otp);
    }
}
