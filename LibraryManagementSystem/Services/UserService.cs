using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.UserDto;
using LibraryManagementSystem.Services.Interfaces;

namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<User?> CreateUser(CreateUserDto dto)
        { 
            //Checks Email or MobileNumber Existance
            bool exist = _dbContext.Users.Any(u => u.Email == dto.Email || u.PhoneNumber == dto.PhoneNumber);
            if (exist)
            {
                throw new Exception("User Already Exist!!!");
            }
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserDetails(GetUserDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => (!string.IsNullOrWhiteSpace(dto.Email) && u.Email == dto.Email) ||
            (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && u.PhoneNumber == dto.PhoneNumber));

            if (user == null || user.Password != dto.Password) return null;

            return user;


        }
    }
}
