using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;
        public BookService(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Book>> GetAllBooks()
        {

            return await _dbContext.Books.ToListAsync();
        }

        public async Task<Book> CreateBook(CreateBookDto newBook)
        {
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                BookTitle = newBook.BookTitle,
                Author = newBook.Author,
                Isbn = newBook.Isbn,
                Description = newBook.Description,
                AvailableCopies = newBook.AvailableCopies,
                TotalCopies = newBook.TotalCopies,
            };
            if(book == null)
                throw new Exception("Unable to Create Book Please try Again");
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> UpdateBook(Guid id, [FromBody] UpdateBookDto dto)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book == null)
                throw new Exception("Book Not Found");

            if (dto.BookTitle != null) book.BookTitle = dto.BookTitle;
            if (dto.Author != null) book.Author = dto.Author;
            if (dto.Description != null) book.Description = dto.Description;
            if (dto.Isbn != null) book.Isbn = dto.Isbn;

            if (dto.TotalCopies.HasValue)
                book.TotalCopies = dto.TotalCopies.Value;

            if (dto.AvailableCopies.HasValue)
                book.AvailableCopies = dto.AvailableCopies.Value;

            if (dto.CategoryId is not null)
            {
                var category = await _dbContext.Categories.FindAsync(dto.CategoryId.Value);

                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                book.CategoryId = dto.CategoryId.Value;
            }
            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book> DeleteBook(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if(book == null)
                throw new Exception("Book not found");
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }
        public async Task<Book> IncrementAvailableBooks(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
                throw new Exception("Book Not Found");
            if (book.AvailableCopies <= 0)
            {
                throw new Exception("Can't make Available copies as Negative items"); 
            }
            book.AvailableCopies--;
            await _dbContext.SaveChangesAsync();
            //var count = book.AvailableCopies;
            return book;
        }
        public async Task<Book> DecrementAvailableBooks(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book is null) 
                throw new Exception("Book not found");

            if (book.AvailableCopies + 1 > book.TotalCopies)
                throw new Exception("Can't make Available copies as More than Total Copies items");

            book.AvailableCopies++;
            await _dbContext.SaveChangesAsync();

            return book;
        }
<<<<<<< Updated upstream
=======

        public async Task<List<Book>?> GetBooksByCategory(Guid categoryId)
        {
            var books = await _dbContext.Books.Where(b => b.CategoryId == categoryId).ToListAsync();
            
            if(books == null  || !books.Any())
            {
                throw new Exception("Can't Find books in this Category");
            }

            return books;
        }
>>>>>>> Stashed changes
            
        
    }
}
