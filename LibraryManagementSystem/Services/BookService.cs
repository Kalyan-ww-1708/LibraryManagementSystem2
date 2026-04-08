using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Services.Interfaces;


namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooks()
        {

            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> CreateBook(CreateBookDto newBook){
            var book = new Book(){
                Id = Guid.NewGuid(),
                BookTitle = newBook.BookTitle,
                Author = newBook.Author,
                Isbn = newBook.Isbn,
                Description = newBook.Description,
                AvailableCopies = newBook.AvailableCopies,
                TotalCopies = newBook.TotalCopies,
                ImageUrl = newBook.ImageUrl
            };
            if (newBook.TotalCopies < newBook.AvailableCopies){
                throw new InValidException("Available copies cannot exceed total copies");
            }
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> UpdateBook(Guid id, UpdateBookDto dto){
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new NotFoundException("Book Not Found");

            var totalCopies = dto.TotalCopies ?? book.TotalCopies;
            var availableCopies = dto.AvailableCopies ?? book.AvailableCopies;

            if (availableCopies > totalCopies)
                throw new InValidException("Available copies cannot exceed total copies");

            if (dto.BookTitle != null) book.BookTitle = dto.BookTitle;
            if (dto.Author != null) book.Author = dto.Author;
            if (dto.Description != null) book.Description = dto.Description;
            if (dto.Isbn != null) book.Isbn = dto.Isbn;
            if (dto.ImageUrl != null) book.ImageUrl = dto.ImageUrl;

            book.TotalCopies = totalCopies;
            book.AvailableCopies = availableCopies;

            if (dto.CategoryId is not null){
                var category = await _bookRepository.GetByCategoryAsync(dto.CategoryId.Value);
                if (category == null)
                    throw new NotFoundException("Category not found");
                book.CategoryId = dto.CategoryId.Value;
            }
            await _bookRepository.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> DeleteBook(Guid id)
        {
            //var book = await _dbContext.Books.FindAsync(id);
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new  NotFoundException("Book Not Found");
            await _bookRepository.DeleteAsync(book);
            await _bookRepository.SaveChangesAsync();
            return book;
        }
        
        public async Task<Book> IncrementAvailableBooks(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book is null)
                throw new NotFoundException("Book not found");

            if (book.AvailableCopies >= book.TotalCopies)
                throw new InValidException("Available copies cannot exceed total copies");

            book.AvailableCopies++;
            await _bookRepository.SaveChangesAsync();

            return book;
        }
        public async Task<Book> DecrementAvailableBooks(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book is null)
                throw new NotFoundException("Book not found");

            if (book.AvailableCopies <= 0)
                throw new InValidException("No available copies");

            book.AvailableCopies--;
            await _bookRepository.SaveChangesAsync();

            return book;
        }


        public async Task<List<Book>> GetBooksByCategory(Guid categoryId)
        {
            var books = await _bookRepository.GetByCategoryAsync(categoryId);
            
            if(books == null  || !books.Any())
            {
                throw new NotFoundException("Can't Find books in this Category");
            }

            return books;
        }   
        public async Task<List<Book>> GetLimitedBooks(int limit)
        {
            var books = await _bookRepository.GetLimitedAsync(limit);
            return books;

        }
        public async Task<int> GetBooksCount()
        {
            var count = await _bookRepository.GetCountAsync();
            return count;
        }
    }
}
