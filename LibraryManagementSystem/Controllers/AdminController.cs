using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.AdminDto;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try{
                var admin = await _adminService.RegisterAdmin(dto);
                return Ok(admin);
            }
            catch(ConflictException e){
                return Conflict(new { message = e.Message });
            }
            catch(Exception e){
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto dto)
        {
            try{
                var admin = await _adminService.LoginAdmin(dto);
                return Ok(admin);
            }
            catch (NotFoundException e){
                return NotFound(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }

        [Route("tfa")]
        [HttpPost]
        public async Task<IActionResult> TfaAdmin(VerifyOtpDto dto)
        {
            try{
                var admin = await _adminService.TfaAdmin(dto);
                return Ok(admin);
            }catch (UnauthorizedException e){
                return Unauthorized(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
