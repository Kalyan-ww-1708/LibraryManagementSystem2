using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryContext;
        public CategoryController(ICategoryService CategoryService)
        {
            _categoryContext = CategoryService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            try{
                var categories = await _categoryContext.GetAllCategory();
                return Ok(categories);
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try{
                var category = await _categoryContext.GetCategoryById(id);
                return Ok(category);
            }
            catch(NotFoundException e){
                return NotFound(new {message = e.Message});
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }


        [HttpGet]
        [Route("cat/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            try{
                var categories = await _categoryContext.GetCategoryByName(name);
     
                return Ok(categories);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e){
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            try{
                var category = await _categoryContext.CreateCategory(dto);
                return CreatedAtAction(nameof(GetCategoryById),
                new { Id = category.CategoryId },
                category);
            }
            catch (ConflictException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}


