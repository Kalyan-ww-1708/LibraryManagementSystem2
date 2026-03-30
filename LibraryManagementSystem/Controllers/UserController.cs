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
            try
            {
                var user = _userService.LoginUser(dto);
                return Ok(user);
            }catch(Exception e)
            {
                return BadRequest(e.Message);

            }
           
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        {
            var user = await _userService.RegisterUser(dto);
            if(user is null)
            {
                return BadRequest("Request Failed ");
            }
            return Ok(user);
        }
    }
}
