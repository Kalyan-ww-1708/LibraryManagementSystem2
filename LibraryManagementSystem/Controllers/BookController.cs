
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> GetAllBooks(){
            try{
                var books = await _bookService.GetAllBooks();
               return Ok(books);

            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("li")]
        public async Task<IActionResult> getLimitedBooks(int limit)
        {
            try{
                var books = await _bookService.GetLimitedBooks(limit);
                return Ok(books);
            }
            catch(InValidException e){
                return Conflict(new { message = e.Message });
            }
            catch(Exception e){
                return BadRequest(new { message = e.Message });
            }
        }


        [HttpGet]
        [Route("cat/{categoryId:guid}")]
        public async Task<IActionResult> GetBooksByCategory(Guid categoryId) {

            try{
                var books = await _bookService.GetBooksByCategory(categoryId);
                if (books is null) 
                    return NotFound("Data Not Found please try again");
                return Ok(books);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }  
        }


        //POST https://localhost:7033/api/book
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook(CreateBookDto dto)
        {
            try
            {
                var NewBook = await _bookService.CreateBook(dto);
                if (NewBook is null) 
                    return NotFound("Can't create the request please try again");
                return Ok(NewBook);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }


        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(Guid id, UpdateBookDto dto)
        {
            try{
                var updatedBook = await _bookService.UpdateBook(id, dto);
                return Ok(updatedBook);
            }catch(NotFoundException e){
                return NotFound(new { message = e.Message });
            }catch(InValidException e) { 
                return NotFound(new { message = e.Message }); 
            }
            catch(Exception e){
                return BadRequest(new { message = e.Message } );
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try{
                var book = await _bookService.DeleteBook(id);
                if (book is null) 
                    return NotFound("Book Not Found");
                return Ok(book);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Route("more/{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> IncrementBooks(Guid id)
        {
            try{
                var book = await _bookService.IncrementAvailableBooks(id);
                return Ok(book);
            }
            catch (NotFoundException e){
                return NotFound(new { message = e.Message });
            }
            catch (InValidException e){
                return NotFound(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("less/{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DecrementAvailableBooks(Guid id)
        {
            try {
                var book = await _bookService.DecrementAvailableBooks(id);
                return Ok(book);
            }
            catch (NotFoundException e){
                return NotFound(new { message = e.Message });
            }
            catch (InValidException e){ 
                return NotFound(new { message = e.Message });
            }
            catch (Exception e) { 
                return BadRequest(e.Message);
            }   
        }
    }
}