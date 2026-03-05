using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryController(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;

        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategory(Guid Id)
        {
            var category = await _dbContext.Categories.ToListAsync();
            if (category is null)
            {
                return NotFound();
            }
            return Ok(category);
        }



        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetCategory(Guid Id)
        {
            var category = await _dbContext.Categories.FindAsync(Id);
            if (category is null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto NewCat)
        {
            if (await _dbContext.Categories.AnyAsync(c => c.CategoryName == NewCat.CategoryName))
            {
                return BadRequest("Category already exists");
            }

            var CategoryEntity = new Category()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = NewCat.CategoryName
            };

            await _dbContext.Categories.AddAsync(CategoryEntity);
            int res = await _dbContext.SaveChangesAsync();

            if(res > 0)
            {
                return Ok(CategoryEntity);
            }
            return BadRequest("Failed to create category");

        }

       
    }
}


