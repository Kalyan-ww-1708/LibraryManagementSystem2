using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using FluentAssertions;

namespace LibraryManagementSystem.UnitTests.RepositoryTest
{
    public class BookRepositoryTest
    {
        private readonly ApplicationDbContext _dbContext;

        //For Dummy Database Should also load InMemory Package
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
        public BookRepositoryTest()
        {
            _dbContext = GetDbContext();
        }

        [Fact]
        public async Task GetAllAsync_WillReturnAllBooks()
        {
            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var books = new List<Book>{
                new Book
                {
                    Id = Guid.NewGuid(),
                    BookTitle = "Book1",
                    Author = "Author1",
                    Description = "Desc",
                    Isbn = "123",
                    AvailableCopies = 1,
                    TotalCopies = 5
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    BookTitle = "Book2",
                    Author = "Author2",
                    Description = "Desc",
                    Isbn = "456",
                    AvailableCopies = 2,
                    TotalCopies = 6
                }
            
            };
            await dbContext.Books.AddRangeAsync(books);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetAllAsync();

            res.Should().HaveCount(2);
            res.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetAllAsync_WillReturnEmptyWithNoBooks()
        {
            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);
            var books = new List<Book>
            {

            };
            await dbContext.Books.AddRangeAsync(books);
            await dbContext.SaveChangesAsync();
            var res = await repo.GetAllAsync();

            res.Should().HaveCount(0);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenExists()
        {

            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 1,
                TotalCopies = 5
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            var result = await repo.GetByIdAsync(book.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(book.Id);
        }

        [Fact]
        public async Task GetIdByAsync_ShouldReturnNull_WithInValidId()
        {

            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var res = await repo.GetByIdAsync(Guid.NewGuid());

            res.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnBook_WithValidData()
        {

            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var book = new Book()
            {
                Id = Guid.NewGuid(),
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 1,
                TotalCopies = 5
            };
            await repo.AddAsync(book);
            await repo.SaveChangesAsync();

            var res = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            res.Should().NotBeNull();
            res.BookTitle.Should().Be("Test");
        }

        [Fact]
        public async Task GetBookByTitleAsync_ShouldReturnBook_WhenExists()
        {
            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var book = new Book
            {
               Id = Guid.NewGuid(),
                BookTitle = "Clean Code",
                Author = "Robert C. Martin",
                Isbn = "111",
                Description = "Hello",
                AvailableCopies = 2,
                TotalCopies = 5
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetBookByTitleAsync("Clean Code");
            res.Should().NotBeNull();
            res.BookTitle.Should().Be("Clean Code");
        }

        [Fact]
        public async Task GetBookByTitleAsync_ShouldReturnNull_WhenDoesNotExists()
        {
            var dbcontext = GetDbContext();
            var repo = new BookRepository(dbcontext);

            var result = await repo.GetBookByTitleAsync("Unknown");

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                BookTitle = "Delete Me",
                Author = "Author",
                Description = "Hello hi",
                Isbn = "333",
                AvailableCopies = 1,
                TotalCopies = 2
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            await repo.DeleteAsync(book);
            await repo.SaveChangesAsync();

            var res = await dbContext.Books.FindAsync(book.Id);

            res.Should().BeNull();
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateBook()
        {
            var dbContext = GetDbContext();
            var repo = new BookRepository(dbContext);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                BookTitle = "Old Title",
                Author = "Author",
                Description = "Hello hi",
                Isbn = "444",
                AvailableCopies = 1,
                TotalCopies = 2
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            book.BookTitle = "Updated Title";

            await repo.UpdateAsync(book);
            await repo.SaveChangesAsync();

            var res = await dbContext.Books.FindAsync(book.Id);

            res.BookTitle.Should().Be("Updated Title");
        }
    }
}
