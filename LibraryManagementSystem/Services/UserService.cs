using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IDistributedCache _cacheService;
        public UserService(ApplicationDbContext DbContext, ITokenService tokenService, IPasswordService passwordService,IOtpService otpService, IEmailService emailService, IDistributedCache cacheService)
        {
            _dbContext = DbContext;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _emailService = emailService;
            _otpService = otpService;
            _cacheService = cacheService;
        }

        // This is for normal registration without Otp
        //public async Task<User?> RegisterUser(CreateUserDto dto)
        //{ 
        //    //Checks Email or MobileNumber Existance
        //    bool exist = _dbContext.Users.Any(u => u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);
        //    if (exist)
        //    {
        //        throw new ConflictException("User Already Exist!!!");
        //    }
        //    var otp = _otpService.GenerateOtp();
        //    var hashedOtp = BCrypt.Net.BCrypt.HashPassword(otp);

        //    // Store OTP
        //    await _cacheService.SetStringAsync(dto.Email, hashedOtp, new DistributedCacheEntryOptions
        //    {
        //        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        //    });


        //    await _emailService.SendOtpToMail(dto.Email,otp);
        //    var hashedPassword = _passwordService.HashPassword(dto.Password);
        //    var user = new User()
        //    {
        //        UserId = Guid.NewGuid(),
        //        UserName = dto.UserName,
        //        Email = dto.Email,
        //        PhoneNumber = dto.PhoneNumber,
        //        Password = hashedPassword,
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    await _dbContext.Users.AddAsync(user);
        //    await _dbContext.SaveChangesAsync();
        //    return user;
        //}

        public async Task<string> RegisterUser(CreateUserDto dto)
        {
            bool exist = await _dbContext.Users.AnyAsync(u => u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);
            if (exist)
                throw new ConflictException("User Already Exists!");

            var otp = _otpService.GenerateOtp();
            var hashedOtp = BCrypt.Net.BCrypt.HashPassword(otp);

            await _cacheService.SetStringAsync(dto.Email, hashedOtp, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            // Store user data temporarily and saves that in cache along with hashed Otp
            var tempUser = JsonSerializer.Serialize(dto);

            await _cacheService.SetStringAsync($"user_{dto.Email}", tempUser,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            await _emailService.SendOtpToMail(dto.Email, otp);
            return "Otp Sent";
        }

        public async Task<User> VerifyUser(UserVerifyOtpDto dto)
        {
            //Gets otp based on email from cache memory and unhash it.
            var storedHashedOtp = await _cacheService.GetStringAsync(dto.Email);

            if (storedHashedOtp == null)
                throw new Exception("OTP expired");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Otp, storedHashedOtp);

            if (!isValid)
                throw new Exception("Invalid OTP");

            var userDataJson = await _cacheService.GetStringAsync($"user_{dto.Email}");

            if (userDataJson == null)
                throw new Exception("Session expired");

            var userDto = JsonSerializer.Deserialize<CreateUserDto>(userDataJson);

            var hashedPassword = _passwordService.HashPassword(userDto.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = userDto.UserName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            await _cacheService.RemoveAsync(dto.Email);
            await _cacheService.RemoveAsync($"user_{dto.Email}");

            return user;
        }

        public async Task<UserLoginResponseDto?> LoginUser(GetUserDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                (!string.IsNullOrWhiteSpace(dto.Identifier) && u.Email == dto.Identifier) ||
                (!string.IsNullOrWhiteSpace(dto.Identifier) && u.PhoneNumber == dto.Identifier));
      
            if (user == null || !_passwordService.VerifyPassword(dto.Password, user.Password))
                throw new UnauthorizedException("Invalid Credentials");

            var token = _tokenService.GenerateUserToken(user);

            return new UserLoginResponseDto
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email
            };
        }
        public async Task<int> GetUsersCount()
        {
            var count = await _dbContext.Users.CountAsync();
            return count;
        }




        public async Task<string> ForgotPassword(SendEmailDto dto)
        {
            bool exist = await _dbContext.Users.AnyAsync(u => u.Email == dto.Email);
            if (!exist)
                throw new NotFoundException("User Doesn't Exists!");

            var otp = _otpService.GenerateOtp();
            var hashedOtp = BCrypt.Net.BCrypt.HashPassword(otp);

            await _cacheService.SetStringAsync(dto.Email, hashedOtp, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            // Store user data temporarily and saves that in cache along with hashed Otp

            await _emailService.SendOtpToMail(dto.Email, otp);
            return "Otp Sent";
        }

        public async Task<string> VerifyForgotPasswordOtp(UserVerifyOtpDto dto)
        {
            //Gets otp based on email from cache memory and unhash it.
            var storedHashedOtp = await _cacheService.GetStringAsync(dto.Email);

            if (storedHashedOtp == null)
                throw new Exception("OTP expired");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Otp, storedHashedOtp);

            if (!isValid)
                throw new Exception("Invalid OTP");


            await _cacheService.SetStringAsync($"verified_{dto.Email}", "true",
                 new DistributedCacheEntryOptions
                 {
                     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                 });

            return "OTP Verified";
        }
        public async Task<string> ResetPassword(ResetPasswordDto dto)
        {
            var Verified = await _cacheService.GetStringAsync($"verified_{dto.Email}");

            if (Verified == null) throw new Exception("Otp Expired Please Try Again");

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new NotFoundException("User not found");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveAsync(dto.Email);
            await _cacheService.RemoveAsync($"verified_{dto.Email}");

            return "Password reset successfully";
        }
    }
}
