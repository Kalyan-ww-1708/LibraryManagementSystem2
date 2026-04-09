using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Repository;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class CategoryService :ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await _categoryRepository.GetAllAsync();
           
        }
        public async Task<Category?> GetCategoryById(Guid id)
        {

            var category = await _categoryRepository.GetByIdAsync(id);
            if(category == null){
                throw new NotFoundException("Can't find Category");
            }
            return category;          
        }
        public async Task<Category> CreateCategory(CreateCategoryDto dto)
        {
            var exist = await _categoryRepository.GetByNameAsync(dto.CategoryName);

            if(exist != null){ 
                throw new ConflictException("Category Already Exist");
            }
            var category = new Category() { CategoryName = dto.CategoryName };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> GetCategoryByName(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);

            if (category == null){
                throw new NotFoundException("Cant Find this Category Please try Again");
            }
            return category;
        }
     
    }
}
