using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext){

            _dbContext = dbContext;
        }
        public async Task AddAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
        }

        public async Task DeleteAsync(Book book)
        {
            _dbContext.Books.Remove(book);
            await Task.CompletedTask;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _dbContext.Books.Include(b=>b.Categories).ToListAsync();
        }

        public async Task<List<Book>> GetByCategoryAsync(Guid categoryId)
        {
            return await _dbContext.Books.Where(b => b.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Books.FindAsync(id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Books.CountAsync();
        }

        public async Task<List<Book>> GetLimitedAsync(int limit)
        {
            return await _dbContext.Books.Take(limit).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _dbContext.Books.Update(book);
            await Task.CompletedTask; 
        }
        public async Task<Book?> GetBookByTitleAsync(string bookTitle) {

            return await _dbContext.Books.FirstOrDefaultAsync(b => b.BookTitle == bookTitle);
        
        }
    }
}
