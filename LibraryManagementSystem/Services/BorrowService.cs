using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BorrowDto;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly ApplicationDbContext _dbContext;
        public BorrowService(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<List<Borrow>> GetBorrowList()
        {
            var borrows = await _dbContext.Borrows.Include(b => b.Book).Include(b => b.User).ToListAsync();

            return borrows;
        }
        public async Task<Borrow> CreateBorrow(CreateBorrowDto dto)
        {
            var book = await _dbContext.Books.FindAsync(dto.BookId);
            if (book == null)
                throw new Exception("Book not found");
            var user = await _dbContext.Users.FindAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found");
            if (book.AvailableCopies <= 0)
                throw new Exception("Book is not available");

            var borrow = new Borrow
            {
                BookId = dto.BookId,
                UserId = dto.UserId,
                BorrowDate = dto.BorrowDate,
                DueDate = dto.DueDate,
                Book = book,
                User = user
            };

            await _dbContext.Borrows.AddAsync(borrow);

            book.AvailableCopies--;

            await _dbContext.SaveChangesAsync();

            return borrow;
        }
    }
}
