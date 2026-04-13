using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBorrowList()
        {
            try{
                var borrowList = await _borrowService.GetBorrowList();
                return Ok(borrowList);
            }
            catch (NotFoundException e){ 
                return NotFound(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("b/{borrowId:guid}")]
        public async Task<IActionResult> GetBorrowWithId(Guid borrowId)
        {
            try
            {
                var borrowList = await _borrowService.GetBorrowWithId(borrowId);
                if (borrowList is null)
                    return NotFound("No borrowList is Found for this book ");
                return Ok(borrowList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("{bookId:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBorrowListByBookId(Guid bookId)
        {
            try{
                var borrowList = await _borrowService.GetBorrowListByBookId(bookId);
                if (borrowList is null)
                    return NotFound("No borrowList is Found for this book ");
                return Ok(borrowList);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }    
        }

        [HttpGet]
        [Route("user/{userId:guid}")]
        public async Task<IActionResult> GetBorrowListByUserId(Guid userId)
        {
            try{
                var borrowList = await  _borrowService.GetBorrowListByUserId(userId);
                if (borrowList is null)
                    return NotFound("No borrowList is Found for this book");
                return Ok(borrowList);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }       
        }
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetBooksCount()
        {
            try{
                var count = await _borrowService.GetBorrowCount();
                return Ok(count);
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet("recent")]
        public async Task<IActionResult> GetLatestBorrows()
        {
            try{
                var result = await _borrowService.GetRecentBorrow();
                return Ok(result);
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBorrow(CreateBorrowDto dto)
        {
            try{
                var borrow = await _borrowService.CreateBorrow(dto);
                return Ok(borrow);
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
        [Route("updateborrow/{borrowId:guid}")]
        public async Task<IActionResult> AddReturnDate(Guid borrowId, [FromBody] UpdateBorrowDto dto)
        {
            try
            {
                var borrow = await _borrowService.AddReturnDate(borrowId, dto);
                return Ok(borrow);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Route("approve-return/{borrowId:guid}")]
        //2eea1db7-0e56-40b0-72a3-08de90a1f7ea
        public async Task<IActionResult> ApproveReturnRequest(Guid borrowId)
        {
            try{
                var borrow = await _borrowService.ApproveReturnRequest(borrowId);
                return Ok(borrow);
            }
            catch(InValidException e){
                return Conflict(e.Message);
            }
            catch(NotFoundException e){
                return NotFound(e.Message);
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }   
        }


        [HttpGet]
        [Route("approval")]
        public async Task<IActionResult> BorrowListForApprovals()
        {
            try{
                var result = await _borrowService.BorrowListForApprovals(); 
                return Ok(result);
            }
            catch(NotFoundException e){
                return NotFound(new { message = e.Message });
            }

            catch(Exception e){
                return BadRequest(new { message = e.Message });
            }
            
        }

        [HttpPatch]
        [Route("updatedue/{borrowId:guid}")]
        public async Task<IActionResult> ExtendDueDate(Guid borrowId,[FromBody] UpdateBorrowDto dto)
        {
            try{
                var borrow = await _borrowService.ExtendDueDate(borrowId, dto);
                return Ok(borrow);
            }
            catch (NotFoundException e){
                return NotFound(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet]
        [Route("download-excel")]
        public async Task<IActionResult> DownloadBorrows()
        {
            var fileBytes = await _borrowService.DownloadBorrowList();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "BorrowList.xlsx"
            );
        }
        [HttpPut]
        [Route("approve/{borrowId:guid}")]
        public async Task<IActionResult> ApproveBorrowRequest(Guid borrowId )
        {
            try{
                var borrow = await _borrowService.ApproveBorrowRequest(borrowId);
                return Ok(borrow);
            }
            catch (NotFoundException e){
                return NotFound(new { message = e.Message });
            }
            catch (InValidException e){
                return Conflict(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPut]
        [Route("reject/{borrowId:guid}")]
        public async Task<IActionResult> RejectBorrowRequest(Guid borrowId) {
            try{
                var borrow = await _borrowService.RejectBorrowRequest(borrowId);
                return Ok(borrow);
            }
            catch (NotFoundException e){
                return NotFound(new { message = e.Message });
            }
            catch (InValidException e){
                return Conflict(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpGet]
        [Route("return-req")]
        public async Task<IActionResult> GetReturnBorrowsAsync()
        {
            try
            {
                var borrowList = await _borrowService.GetReturnBorrowsAsync();
                return Ok(borrowList);
            }catch(Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
