using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
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

        public async Task<Book?> CreateBook(CreateBookDto newBook)
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
            if (newBook.TotalCopies < newBook.AvailableCopies)
            {
                throw new Exception("Available copies cannot exceed total copies");
            }
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> UpdateBook(Guid id, [FromBody] UpdateBookDto dto)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null) 
                return null;

            if (dto.BookTitle != null) book.BookTitle = dto.BookTitle;
            if (dto.Author != null) book.Author = dto.Author;
            if (dto.Description != null) book.Description = dto.Description;
            if (dto.Isbn != null) book.Isbn = dto.Isbn;

            if (dto.TotalCopies.HasValue)
                book.TotalCopies = dto.TotalCopies.Value;

            if (dto.AvailableCopies.HasValue)
                book.AvailableCopies = dto.AvailableCopies.Value;

            if (dto.TotalCopies < dto.AvailableCopies)
            {
                throw new Exception("Available copies cannot exceed total copies");
            }

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

        public async Task<Book?> DeleteBook(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null) return null;
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }
        
        public async Task<Book> IncrementAvailableBooks(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book is null)
                throw new Exception("Book not found");

            if (book.AvailableCopies >= book.TotalCopies)
                throw new Exception("Available copies cannot exceed total copies");

            book.AvailableCopies++;
            await _dbContext.SaveChangesAsync();

            return book;
        }
        public async Task<Book> DecrementAvailableBooks(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book is null)
                throw new Exception("Book not found");

            if (book.AvailableCopies <= 0)
                throw new Exception("No available copies");

            book.AvailableCopies--;
            await _dbContext.SaveChangesAsync();

            return book;
        }


        public async Task<List<Book>> GetBooksByCategory(Guid categoryId)
        {
            var books = await _dbContext.Books.Where(b => b.CategoryId == categoryId).ToListAsync();
            
            if(books == null  || !books.Any())
            {
                throw new Exception("Can't Find books in this Category");
            }

            return books;
        }   
        
    }
}
