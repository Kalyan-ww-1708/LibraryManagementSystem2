using LibraryManagementSystem.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
namespace LibraryManagementSystem.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendOtpToMail(string email, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Library Management", "kalyan.arutla@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "Otp Verification";
            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP is: {otp}"
            };

            var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("kalyan.arutla@gmail.com", "ioer yfcv cfyx znex");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }   
}
