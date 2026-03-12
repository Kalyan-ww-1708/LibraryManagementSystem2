using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.AdminDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        public AdminService(ApplicationDbContext DbContext, ITokenService tokenService, IOtpService otpService, IEmailService emailService)
        {
            _dbContext = DbContext;
            _tokenService = tokenService;
            _otpService = otpService;
            _emailService = emailService;

        }

        public async Task<Admin?> RegisterAdmin(CreateAdminDto dto) {

            var exist = await _dbContext.Admins.FirstOrDefaultAsync(u => dto.Email == u.Email);
            if (exist !=null)
            {
                return null;
            }
            var admin = new Admin()
            {
                AdminId = Guid.NewGuid(),
                AdminName = dto.AdminName,
                Email = dto.Email,
                Password = dto.Password,
            };
            await _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();

            return admin;
        
        }
        public async Task<string?> LoginAdmin(LoginDto dto)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(a =>
                    a.Email == dto.Identifier ||
                    a.AdminName == dto.Identifier);

            if (admin == null)
                return null;

            if (dto.Password != admin.Password)
                return null;


            admin.Otp = _otpService.GenerateOtp();
            admin.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            await _dbContext.SaveChangesAsync();
            _emailService.SendOtpToMail(admin.Email, admin.Otp);

            return "Otp Sent to mail please confirm two factor Authentication";

            
        }
        public async Task<LoginResponseDto?> TfaAdmin(VerifyOtpDto dto)
        {
            var admin =await _dbContext.Admins.FirstOrDefaultAsync(u=> u.Email == dto.Email);
            
            if (admin== null || admin.Otp != dto.Otp || admin.OtpExpiry == null || admin.OtpExpiry < DateTime.UtcNow)
            {
                return null;
            }
            var token = _tokenService.GenerateToken(admin);
            return new LoginResponseDto
            {
                Token = token,
                AdminName = admin.AdminName,
                Email = admin.Email
            };
        }
    }
}
