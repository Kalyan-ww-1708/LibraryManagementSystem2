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
        private readonly IPasswordService _passwordService;
        public AdminService(ApplicationDbContext DbContext, ITokenService tokenService, IOtpService otpService, IEmailService emailService, IPasswordService passwordService)
        {
            _dbContext = DbContext;
            _tokenService = tokenService;
            _otpService = otpService;
            _emailService = emailService;
            _passwordService = passwordService;

        }

        public async Task<Admin?> RegisterAdmin(CreateAdminDto dto) 
        {
            //Check Admin Name also along with Email
            var exist = await _dbContext.Admins.FirstOrDefaultAsync(u =>
            u.Email == dto.Email ||u.AdminName == dto.AdminName);
            if (exist !=null)
                throw new ConflictException("Admin Already Exist");
            var hashedPassword = _passwordService.HashPassword(dto.Password);
            var admin = new Admin(){
                AdminId = Guid.NewGuid(),
                AdminName = dto.AdminName,
                Email = dto.Email,
                Password = hashedPassword,
            };
            await _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();

            return admin;
        }
        public async Task<string?> LoginAdmin(LoginDto dto)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(a =>a.Email == dto.Identifier ||
                    a.AdminName == dto.Identifier);

            if (admin == null)
                throw new NotFoundException("Admin Not Found");

            if (!_passwordService.VerifyPassword(dto.Password,admin.Password))
                throw new UnauthorizedException("Invalid Credentials");

            admin.Otp = _otpService.GenerateOtp();
            admin.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            await _dbContext.SaveChangesAsync();
            await _emailService.SendOtpToMail(admin.Email, admin.Otp);

            return admin.Email;  
        }
        public async Task<LoginResponseDto?> TfaAdmin(VerifyOtpDto dto)
        {
            var admin =await _dbContext.Admins.FirstOrDefaultAsync(u=> u.Email == dto.Email);
            
            if (admin== null || admin.Otp != dto.Otp || admin.OtpExpiry == null || admin.OtpExpiry < DateTime.UtcNow){
                throw new UnauthorizedException("Invalid Credentials");
            }
            var token = _tokenService.GenerateAdminToken(admin);
            return new LoginResponseDto
            {
                Token = token,
                AdminName = admin.AdminName,
                Email = admin.Email
            };
        }
    }
}
