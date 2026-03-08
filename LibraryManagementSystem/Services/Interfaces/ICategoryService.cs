using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryById(Guid Id);
        Task<Category> CreateCategory(CreateCategoryDto dto);
        Task<List<Category>> GetAllCategory();
        Task<Category?> GetCategoryByName(string name);
        
    }
}
