using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BookDto;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ICategoryService
    {
<<<<<<< Updated upstream
        Task<Category> GetCategoryById(Guid Id);
=======
        Task<Category?> GetCategoryById(Guid id);
>>>>>>> Stashed changes
        Task<Category> CreateCategory(CreateCategoryDto dto);
        Task<List<Category>> GetAllCategory();
    }
}
