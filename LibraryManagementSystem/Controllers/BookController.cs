using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public BookController(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;

        }

        //GET https://localhost:7033/api/book
        [HttpGet]
        public async Task<IActionResult> getAllBooks()
        {
            var books = await _dbContext.Books.ToListAsync();
            return Ok(books);
        }

        //POST https://localhost:7033/api/book
        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto NewBook)
        {
            var BookEntity = new Book()
            {
                Id = Guid.NewGuid(),
                BookTitle = NewBook.BookTitle,
                Author = NewBook.Author,
                Isbn = NewBook.Isbn,
                Description = NewBook.Description,
                AvailableCopies = NewBook.AvailableCopies,
                TotalCopies = NewBook.TotalCopies,
            };
            await _dbContext.Books.AddAsync(BookEntity);
            await _dbContext.SaveChangesAsync();
            return Ok(BookEntity);
        }
    }
}
