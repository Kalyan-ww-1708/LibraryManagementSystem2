using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BorrowDto;
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
                return Ok(borrow);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
