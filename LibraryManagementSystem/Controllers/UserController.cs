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
                return BadRequest(new { message = e.Message });
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
                return NotFound(new { message = e.Message });
            }
            catch(Exception e){
                return BadRequest(new {message = e.Message});
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        {
            try{
                var user = await _userService.RegisterUser(dto);
                return Ok(user);
            }
            catch(ConflictException e){
                return Conflict(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }

        }
    }
}
