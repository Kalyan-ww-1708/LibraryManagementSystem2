using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService UserService )
        {
            _userService = UserService;

        }
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetBooksCount()
        {
            try
            {
                var count = await _userService.GetUsersCount();
                return Ok(count);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(GetUserDto dto)
        {
            try{
                var user = _userService.LoginUser(dto);
                return Ok(user);
            }
            catch (NotFoundException e){
                return NotFound(e.Message);
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
        }
        // This is for normal Registration
        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        //{
        //    try{
        //        var user = await _userService.RegisterUser(dto);
        //        return Ok(user);
        //    }
        //    catch(ConflictException e){
        //        return Conflict(e.Message);
        //    }
        //    catch (Exception e){
        //        return BadRequest(e.Message);
        //    }
        //}
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        {
            try{
                var user = await _userService.RegisterUser(dto);
                return Ok(user);
            }
            catch (ConflictException e){
                return Conflict(e.Message);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] UserVerifyOtpDto dto)
        {
            try{
                var user = await _userService.VerifyUser(dto);
                return Ok(user);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("send-otp")]
        public async Task<IActionResult> OtpForForgetOtp([FromBody] SendEmailDto dto)
        {
            try
            {
                var user = await _userService.ForgotPassword(dto);
                return Ok(user);
            }catch(NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("verify-forgot-otp")]
        public async Task<IActionResult> VerifyForgotOtp([FromBody] UserVerifyOtpDto dto)
        {
            try
            {
                var user = await _userService.VerifyForgotPasswordOtp(dto);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var user = await _userService.ResetPassword(dto);
                return Ok(user);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
