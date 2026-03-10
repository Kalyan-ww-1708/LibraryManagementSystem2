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
        public async Task<IActionResult> GetUserDetails(GetUserDto dto)
        {
            try
            {
                var user = _userService.GetUserDetails(dto);
                return Ok(user);
            }catch(Exception e)
            {
                return BadRequest(e.Message);

            }
           
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = await _userService.CreateUser(dto);
            if(user is null)
            {
                return BadRequest("Request Failed ");
            }
            return Ok(user);
        }
    }
}
