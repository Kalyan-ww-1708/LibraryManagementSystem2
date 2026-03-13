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
            try
            {
                var categories = await _categoryContext.GetAllCategory();
                if (categories is null)
                    return NotFound("Categories Not Found");
                return Ok(categories);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryContext.GetCategoryById(id);
                if (category is null)
                    return NotFound("Categories Not FOund");
                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("cat/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name) {
            try
            {
                var categories = await _categoryContext.GetCategoryByName(name);
                if (categories is null)
                    return NotFound("Categories Not Found");
                return Ok(categories);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

       [HttpPost]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            try
            {
                var category = await _categoryContext.CreateCategory(dto);
                if (category is null)
                    return NotFound("Categories Not Found");
                return CreatedAtAction(nameof(GetCategoryById),
                new { Id = category.CategoryId },
                category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }            
        }


    }
}


