using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.AdminDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using System.Collections;

namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        public UserService(ApplicationDbContext DbContext, ITokenService tokenService, IPasswordService passwordService)
        {
            _dbContext = DbContext;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<User?> RegisterUser(CreateUserDto dto)
        { 
            //Checks Email or MobileNumber Existance
            bool exist = _dbContext.Users.Any(u => u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);
            if (exist)
            {
                throw new ConflictException("User Already Exist!!!");
            }
            var hashedPassword = _passwordService.HashPassword(dto.Password);
;            var user = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
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
    }
}
