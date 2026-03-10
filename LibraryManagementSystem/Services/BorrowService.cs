using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
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

        public async Task<List<ShowUserDto>> GetBorrowListByBookId(Guid bookId)
        {
            var result = await _dbContext.Borrows.Where(b => b.BookId == bookId).Select(b => new ShowUserDto
            {
                UserName = b.User.UserName,
                Email = b.User.Email,
                PhoneNumber = b.User.PhoneNumber}).ToListAsync();

            if (!result.Any())
                throw new Exception("No books were borrowed with that Id");

            return result;
        }
    }


}
