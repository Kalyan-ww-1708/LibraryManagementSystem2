using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBookService
    {
         Task<List<Book>> GetAllBooks();
         Task<Book> CreateBook(CreateBookDto dto);
<<<<<<< Updated upstream
        Task<Book?> UpdateBook(Guid id, UpdateBookDto dto);
        Task<Book?> DeleteBook(Guid Id);
        Task<Book?> IncrementAvailableBooks(Guid Id);
        Task<Book?> DecrementAvailableBooks(Guid Id);
=======
        Task<Book> UpdateBook(Guid id, UpdateBookDto dto);
        Task<Book> DeleteBook(Guid id);
        Task<Book> IncrementAvailableBooks(Guid id);
        Task<Book> DecrementAvailableBooks(Guid id);
        Task<List<Book>> GetBooksByCategory(Guid categoryId);
>>>>>>> Stashed changes

    }
}
