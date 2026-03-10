using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowList()
        {
            try
            {
                var borrowList = await _borrowService.GetBorrowList();
                if (borrowList is null)
                    return NotFound("No borrowList is Found ");
                return Ok(borrowList);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{bookId:guid}")]
        public async Task<IActionResult> GetBorrowListByBookId(Guid bookId)
        {
            try
            {
                var borrowList = await _borrowService.GetBorrowListByBookId(bookId);
                //if (borrowList is null)
                //    return NotFound("No borrowList is Found for this book ");
                return Ok(borrowList);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }    
        }




        [HttpPost]
        public async Task<IActionResult> CreateBorrow(CreateBorrowDto dto)
        {
            try
            {
                var borrow = await _borrowService.CreateBorrow(dto);
                if (borrow is null)
                    return NotFound("No borrowList is Found for this book ");
                return Ok(borrow);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
