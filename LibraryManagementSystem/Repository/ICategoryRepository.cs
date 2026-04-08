using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid categoryId);
        Task AddAsync(Category category);
        Task DeleteAsync(Category category);
        Task<Category?> GetByNameAsync(string name);
        Task SaveChangesAsync();


    }
}
