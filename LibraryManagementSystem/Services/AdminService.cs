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
        public AdminService(ApplicationDbContext DbContext, ITokenService tokenService)
        {
            _dbContext = DbContext;
            _tokenService = tokenService;

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
        public async Task<LoginResponseDto?> LoginAdmin(LoginDto dto)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(a =>
                    a.Email == dto.Identifier ||
                    a.AdminName == dto.Identifier);

            if (admin == null)
                return null;

            if (dto.Password != admin.Password)
                return null;

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
