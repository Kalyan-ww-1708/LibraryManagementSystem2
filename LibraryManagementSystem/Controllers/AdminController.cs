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

                if (admin == null)
                {
                    return Conflict(new { message = "Admin already exists" });
                }

                return CreatedAtAction(nameof(RegisterAdmin), new { id = admin.AdminId }, admin);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto dto)
        {
            try{
                var admin = await _adminService.LoginAdmin(dto);

                if(admin == null){
                    return Unauthorized(new { message = "Invalid email or password" });
                }
                return Ok(admin);
            }
            catch (Exception e){
                return StatusCode(500, new { message = "Something went wrong please try again"});
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
                    return Unauthorized(new { message = "Two factor Authentication Failed" });
                }
                return Ok(admin);
            }catch(Exception e)
            {
                 return StatusCode(500, new { message = "Something went wrong please try again" });
            }
           

        }
    }
}
