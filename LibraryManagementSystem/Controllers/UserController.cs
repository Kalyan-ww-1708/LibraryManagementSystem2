using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.UserDto;
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
