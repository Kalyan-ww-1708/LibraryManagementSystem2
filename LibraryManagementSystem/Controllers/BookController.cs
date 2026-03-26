
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var books = await _bookService.GetAllBooks();
                if (books is null) 
                    return NotFound("Data Not Found please try again");
                
               return Ok(books);

            }catch(Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpGet]
        [Route("li")]
        public async Task<IActionResult> getLimitedBooks(int limit)
        {
            try
            {
                var books = await _bookService.GetLimitedBooks(limit);
                if (books is null) {
                    return NotFound("Data not found");
                 }
                return Ok(books);
            }
            catch(Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }


        [HttpGet]
        [Route("cat/{categoryId:guid}")]
        public async Task<IActionResult> GetBooksByCategory(Guid categoryId) {

            try
            {
                var books = await _bookService.GetBooksByCategory(categoryId);
                if (books is null) 
                    return NotFound("Data Not Found please try again");
                return Ok(books);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }  
        }


        //POST https://localhost:7033/api/book
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook(CreateBookDto dto)
        {
            try
            {
                var NewBook = await _bookService.CreateBook(dto);
                if (NewBook is null)
                    return Conflict(new { message = "Book already exists" });
                return Ok(NewBook);
            }catch(Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
            
        }


        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(Guid id, UpdateBookDto dto)
        {
            try{
                var updatedBook = await _bookService.UpdateBook(id, dto);
                if (updatedBook is null) 
                    return NotFound("Book Not Found");
                return Ok(updatedBook);
            }catch(Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try{
                var book = await _bookService.DeleteBook(id);
                if (book is null) 
                    return NotFound("Book Not Found");
                return Ok(book);
            }catch(Exception e){
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }
        [HttpPatch]
        [Route("more/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IncrementBooks(Guid id)
        {
            try{
                var book = await _bookService.IncrementAvailableBooks(id);
                if (book is null) 
                    return NotFound("Can't increment Count");
                return Ok(book);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [HttpPatch]
        [Route("less/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DecrementAvailableBooks(Guid id)
        {
            try {
                var book = await _bookService.DecrementAvailableBooks(id);
                if (book is null)
                    return NotFound("Can't increment Count");
                return Ok(book);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }   
        }
    }
}