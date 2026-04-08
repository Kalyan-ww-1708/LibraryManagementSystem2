using LibraryManagementSystem.Context;
using LibraryManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
        }

        public async Task DeleteAsync(Category category)
        {
             _dbContext.Categories.Remove(category);
            await Task.CompletedTask;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            return await _dbContext.Categories.FindAsync(categoryId);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(b => b.CategoryName == name);
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
