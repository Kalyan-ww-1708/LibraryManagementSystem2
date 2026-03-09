using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class CategoryService :ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryService(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await _dbContext.Categories.ToListAsync();
           
        }
<<<<<<< Updated upstream
        public async Task<Category> GetCategoryById(Guid Id)
=======
        public async Task<Category?> GetCategoryById(Guid id)
>>>>>>> Stashed changes
        {
            return await _dbContext.Categories.FindAsync(id);
        }
        public async Task<Category> CreateCategory(CreateCategoryDto dto)
        {
            var exist = _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == dto.CategoryName);

            if(exist != null) { 
                throw new Exception($"User already exists with Id: {exist}");
            }
            var category = new Category() { CategoryName = dto.CategoryName };
            if (category is null)
            {
                throw new Exception("Cant create this Category Please try Again");
            }
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
<<<<<<< Updated upstream
=======
        public async Task<Category?> GetCategoryByName(string name)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);

            if(category == null)
            {
                throw new Exception("Cant Find this Category Please try Again");
            }
            return category;
        }

>>>>>>> Stashed changes
     
    }
}
