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

        public async Task<Book> CreateBook(CreateBookDto NewBook)
        {
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                BookTitle = NewBook.BookTitle,
                Author = NewBook.Author,
                Isbn = NewBook.Isbn,
                Description = NewBook.Description,
                AvailableCopies = NewBook.AvailableCopies,
                TotalCopies = NewBook.TotalCopies,
            };
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            return book;

        }

        public async Task<Book?> UpdateBook(Guid Id, [FromBody] UpdateBookDto dto)
        {
            var book = await _dbContext.Books.FindAsync(Id);

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

            if (dto.CategoryId.HasValue)
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

        public async Task<Book?> DeleteBook(Guid Id)
        {
            var book = await _dbContext.Books.FindAsync(Id);
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }
        public async Task<Book?> IncrementAvailableBooks(Guid Id)
        {
            var book = await _dbContext.Books.FindAsync(Id);
            if(book == null)
            {
                return null ;
            }
            if(book.AvailableCopies <= 0)
            {
                return null; 
            }
            book.AvailableCopies--;
            await _dbContext.SaveChangesAsync();
            //var count = book.AvailableCopies;
            return book;
        }
        public async Task<Book?> DecrementAvailableBooks(Guid Id)
        {
            var book = await _dbContext.Books.FindAsync(Id);
            if (book is null) return null;
            if (book.AvailableCopies + 1 > book.TotalCopies) return null;

            book.AvailableCopies++;
            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<List<Book>?> GetBooksByCategory(Guid categoryId)
        {
            var books = await _dbContext.Books.Where(b => b.CategoryId == categoryId).ToListAsync();
            
            if(books == null  || !books.Any())
            {
                return null;
            }

            return books;
        }
            
        
    }
}
