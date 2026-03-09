
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

<<<<<<< Updated upstream
        
=======
        [HttpGet]
        [Route("cat/{categoryId:guid}")]
        public async Task<IActionResult> GetBooksByCategory(Guid categoryId) {

            try
            {
                var books = await _bookService.GetBooksByCategory(categoryId);
                return Ok(books);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }  
        }
>>>>>>> Stashed changes

        //POST https://localhost:7033/api/book
        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto dto)
        {
            try
            {
                var NewBook = await _bookService.CreateBook(dto);
                return Ok(NewBook);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateBook(Guid id, UpdateBookDto dto)
        {
            try{

                var updatedBook = await _bookService.UpdateBook(id, dto);
                return Ok(updatedBook);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try{
                var book = await _bookService.DeleteBook(id);
                return Ok(book);
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Route("less/{id:guid}")]
        public async Task<IActionResult> IncrementBooks(Guid id)
        {
            try{
                var book = await _bookService.IncrementAvailableBooks(id);
                return Ok(book);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("more/{id:guid}")]
        public async Task<IActionResult> DecrementAvailableBooks(Guid id)
        {
            try {
                var book = await _bookService.DecrementAvailableBooks(id);
                return Ok(book);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

    }
}
