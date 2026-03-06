
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService )
        {
            _bookService = bookService;

        }

        //GET https://localhost:7033/api/book
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        

        //POST https://localhost:7033/api/book
        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto dto)
        {
            var NewBook = await _bookService.CreateBook(dto);
            return Ok(NewBook); 
        }


        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> UpdateBook(Guid Id, UpdateBookDto dto)
        {
            var updatedBook = await _bookService.UpdateBook(Id, dto);
            if (updatedBook is null)
            {
                return NotFound("Data is Not Updated");
            }
            return Ok(updatedBook);
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> DeleteBook(Guid Id)
        {
            var book = await _bookService.DeleteBook(Id);
            if(book is null)
            {
                return NotFound("Book Not found");
            }
            return Ok(book);
        }
        [HttpPatch]
        [Route("less/{Id:Guid}")]
        public async Task<IActionResult> IncrementBooks(Guid Id)
        {
            var book = await _bookService.IncrementAvailableBooks(Id);
            if (book is null)
            {
                return BadRequest("Request Failed");

            }

            return Ok(book);
        }

        [HttpPatch]
        [Route("more/{Id:Guid}")]
        public async Task<IActionResult> DecrementAvailableBooks(Guid Id)
        {
            var book = await _bookService.DecrementAvailableBooks(Id);
            if(book is null)
            {
                return BadRequest("Request Failed");
            }
            return Ok(book);
        }

    }
}
