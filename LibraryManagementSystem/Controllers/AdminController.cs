using LibraryManagementSystem.Dtos.AdminDto;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody]  CreateAdminDto dto)
        {
            try
            {
                var admin = await _adminService.RegisterAdmin(dto);
                if (admin is null)
                {
                    return NotFound("Unable to create Admin");
                }
                return Ok(admin);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto dto)
        {
            try
            {
                var admin = await _adminService.LoginAdmin(dto);
                if (admin == null)
                {
                    return NotFound("Unable to Get Admin");
                }
                return Ok(admin);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("tfa")]
        [HttpPost]
        public async Task<IActionResult> TfaAdmin(VerifyOtpDto dto)
        {
            try
            {
                var admin = await _adminService.TfaAdmin(dto);
                if (admin is null)
                {
                    return Unauthorized("Two factor Authentication Failed");
                }
                return Ok(admin);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
           

        }
    }
}
