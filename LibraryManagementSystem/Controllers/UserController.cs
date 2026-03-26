using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(GetUserDto dto)
        {
            try
            {
                var user = _userService.LoginUser(dto);
                if(user is null)
                {
                    return NotFound("User Not found please try again with proper credentials");
                }
                return Ok(user);
            }catch(Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong please try again" });

            }
           
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var user = await _userService.RegisterUser(dto);
                if (user == null)
                {
                    return Conflict(new { message = "User already exists" });
                }
           
                return CreatedAtAction(nameof(RegisterUser), user);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Something went wrong please try again" });
            }
            
        }
    }
}
