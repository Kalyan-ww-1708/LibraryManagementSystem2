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
                Text = $@"Hello,
                            Your One-Time Password (OTP) is: {otp}
                            This OTP is valid for the next 5 minutes.
                            Please do not share it with anyone for security reasons.
                            If you did not request this, please ignore this email.

                            Regards,
                            Library Management System Team"
            };

            var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("kalyan.arutla@gmail.com", "ioer yfcv cfyx znex");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }   
}
