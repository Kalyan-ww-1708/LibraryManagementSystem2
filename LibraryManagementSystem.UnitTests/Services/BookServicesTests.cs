using FluentAssertions;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LibraryManagementSystem.UnitTests.Services
{
    public class BookServicesTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly BookService _bookService;
        public BookServicesTests()
        {
            _mockRepo = new Mock<IBookRepository>();
            _bookService = new BookService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkWithListOfBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), BookTitle = "Book1", Author = "Author1", Description = "Desc", Isbn = "123", AvailableCopies = 5, TotalCopies = 10 },
                new Book { Id = Guid.NewGuid(), BookTitle = "Book2", Author = "Author2", Description = "Desc", Isbn = "456", AvailableCopies = 3, TotalCopies = 5 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllBooks();

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetAllBooks_ReturnEmpty_WithNoBooks(){

            // Arrange
            var books = new List<Book> { };

            // Act means when Db asked for GetAllAsync data return these books 
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

            var res = await _bookService.GetAllBooks();

            //Assert means Validate
            res.Should().BeEmpty();
        }
        [Fact]
        public async Task CreateBook_ShouldReturnBook_WhenValid()
        {
            // Arrange
            var dto = new CreateBookDto
            {
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 5,
                TotalCopies = 10
            };

            // Act
            var result = await _bookService.CreateBook(dto);

            // Assert
            result.Should().NotBeNull();
            result.BookTitle.Should().Be("Test");

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateBook_ShouldRaiseException_WhenAvailableGreaterThanTotal()
        {
            var dto = new CreateBookDto
            {
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 15, // Greater than totalCopies
                TotalCopies = 10
            };
            Func<Task> act = async () => await _bookService.CreateBook(dto);
            await act.Should().ThrowAsync<InValidException>();

        }

        [Fact]
        public async Task UpdateBook_ShouldThrowNotFound_WhenBookNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            var dto = new UpdateBookDto();

            Func<Task> act = async () => await _bookService.UpdateBook(Guid.NewGuid(), dto);

            await act.Should().ThrowAsync<NotFoundException>();
        }
        [Fact]
        public async Task DeleteBook_ShouldThrowNotFound_WhenBookNotExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Book)null);

            Func<Task> act = async () => await _bookService.DeleteBook(Guid.NewGuid());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DecrementBook_ShouldDecreaseAvailableCopies()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 5,
                TotalCopies = 10
            };
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);

            
            var res =  await _bookService.DecrementAvailableBooks(book.Id);

            res.AvailableCopies.Should().Be(4);


        }
        [Fact]
        public async Task IncrementBook_ShouldIncreaseAvailableCopies()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 1,
                TotalCopies = 10
            };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);

            // Act
            var result = await _bookService.IncrementAvailableBooks(book.Id);

            // Assert
            result.AvailableCopies.Should().Be(2);

            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task Increment_ShouldThrow_WhenExceedsTotalCopies()
        {
            var book = new Book
            {
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 10,
                TotalCopies = 10
            };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync(book);

            Func<Task> act = async () => await _bookService.IncrementAvailableBooks(book.Id);

            await act.Should().ThrowAsync<InValidException>();
        }
        [Fact]
        public async Task Decrement_ShouldThrow_WhenNoCopiesAvailable()
        {
            var book = new Book
            {
                BookTitle = "Test",
                Author = "Author",
                Description = "Desc",
                Isbn = "123",
                AvailableCopies = 0,
                TotalCopies = 10
            };

            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync(book);

            Func<Task> act = async () => await _bookService.DecrementAvailableBooks(book.Id);

            await act.Should().ThrowAsync<InValidException>();
        }
    }
}
