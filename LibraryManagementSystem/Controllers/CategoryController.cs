using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;
using LibraryManagementSystem.Services.Interfaces;
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
            var categories = await _categoryContext.GetAllCategory();
            if(!categories.Any())
            {
                return NotFound("No categories found");
            }
            return Ok(categories);

        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetCategoryById(Guid Id)
        {
            var category = await _categoryContext.GetCategoryById(Id);
            if(category is null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet]
        [Route("cat/{name:string}")]
        public async Task<IActionResult> GetCategoryByName(string name) { 
            var categories = _categoryContext.GetCategoryByName(name);
            if (categories is null)
            {
                return NotFound("Data Not Found");
            }
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            var category = await _categoryContext.CreateCategory(dto);

            return CreatedAtAction(nameof(GetCategoryById),
                new { Id = category.CategoryId },
                category);
        }


    }
}


