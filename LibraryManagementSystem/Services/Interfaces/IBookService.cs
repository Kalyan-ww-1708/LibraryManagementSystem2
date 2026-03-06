using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBookService
    {
         Task<List<Book>> GetAllBooks();
         Task<Book> CreateBook(CreateBookDto dto);
        Task<Book?> UpdateBook(Guid id, UpdateBookDto dto);
        Task<Book?> DeleteBook(Guid Id);
        Task<Book?> IncrementAvailableBooks(Guid Id);
        Task<Book?> DecrementAvailableBooks(Guid Id);

    }
}
