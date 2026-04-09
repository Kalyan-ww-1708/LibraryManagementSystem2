using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Repository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetLimitedAsync(int limit);
        Task<Book?> GetByIdAsync(Guid id);
        Task<List<Book>> GetByCategoryAsync(Guid categoryId);
        Task<int> GetCountAsync();
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task SaveChangesAsync();
        Task<Book?> GetBookByTitleAsync(string bookTitle);
    }
}
