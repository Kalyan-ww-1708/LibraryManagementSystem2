using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBookService
    {
         Task<List<Book>> GetAllBooks();
        Task<List<Book>> GetLimitedBooks(int limit);
         Task<Book?> CreateBook(CreateBookDto dto);
        Task<Book?> UpdateBook(Guid id, UpdateBookDto dto);
        Task<Book?> DeleteBook(Guid id);
        Task<Book> IncrementAvailableBooks(Guid id);
        Task<Book> DecrementAvailableBooks(Guid id);
        Task<List<Book>> GetBooksByCategory(Guid categoryId);
        Task<int> GetBooksCount();


    }
}
