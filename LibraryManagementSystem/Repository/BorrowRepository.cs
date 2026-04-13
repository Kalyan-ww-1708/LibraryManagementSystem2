
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BorrowRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Borrow>> GetAllAsync()
        {
            return await _dbContext.Borrows.Include(b => b.Book).Include(b => b.User).ToListAsync();
        }

        public async Task<Borrow?> GetByIdAsync(Guid borrowId)
        {
            return await _dbContext.Borrows.Include(b => b.Book).Include(b => b.User).FirstOrDefaultAsync(b => b.BorrowId == borrowId);
        }

        public async Task AddAsync(Borrow borrow)
        {
            await _dbContext.Borrows.AddAsync(borrow);
        }

        public async Task UpdateAsync(Borrow borrow)
        {
            _dbContext.Borrows.Update(borrow);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Borrow borrow)
        {
            _dbContext.Borrows.Remove(borrow);
            await Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Borrow>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Borrows.Where(b => b.UserId == userId).Include(b => b.Book).Include(b => b.User).ToListAsync();
        }

        public async Task<List<Borrow>> GetByBookIdAsync(Guid bookId)
        {
            return await _dbContext.Borrows.Where(b => b.BookId == bookId).ToListAsync();
        }

        public async Task<Book?> GetBookByBookId(Guid bookId)
        {
            return await _dbContext.Books.FindAsync(bookId);
        }

        public async Task<User?> GetUserByUserId(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<List<UserLoginResponseDto>> GetBorrowUsersByBookIdAsync(Guid bookId)
        {
            return await _dbContext.Borrows.Where(b => b.BookId == bookId)
                .Select(b => new UserLoginResponseDto
                {
                    UserName = b.User!.UserName,
                    Email = b.User.Email,
                    PhoneNumber = b.User.PhoneNumber
                }).ToListAsync();
        }
        //for Multiple books
        public async Task<bool> ExistanceAsync(Guid userId, Guid bookId)
        {
            return await _dbContext.Borrows.AnyAsync(b => b.UserId == userId && b.BookId == bookId);
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Borrows.Where(u => u.Status == "Approved").CountAsync();
        }

        //FormatException downloading excel sheet details
        public async Task<List<BorrowDetailsDto>> GetCustomBorrowDetailsAsync()
        {
            return await _dbContext.Borrows.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                BookTitle = b.Book!.BookTitle,
                UserName = b.User!.UserName,
                PhoneNumber = b.User.PhoneNumber,
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                ReturnDate = b.ReturnDate
            }).ToListAsync();
        }

        public async Task<List<Borrow>> GetApprovalBorrowsAsync()
        {
            return await _dbContext.Borrows.Where(b => b.Status == "Pending").Include(b => b.Book).Include(b => b.User).ToListAsync();
        }
        public async Task<List<Borrow>> GetReturnBorrowsAsync(){ 
            return await _dbContext.Borrows.Where(b=>b.Status == "Returned Raised").Include(b => b.Book).Include(b => b.User).ToListAsync();
        }
        public async Task<List<Borrow>> GetRecentList()
        {
            return await _dbContext.Borrows.OrderByDescending(b => b.StatusUpdatedAt).Take(10).ToListAsync();
        }
    }
}
