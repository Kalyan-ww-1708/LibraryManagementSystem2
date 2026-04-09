using FluentAssertions;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Dtos.BookDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace LibraryManagementSystem.UnitTests.ControllerTest
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

      
        [Fact]
        public async Task GetAllCategory_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetAllCategory())
                        .ReturnsAsync(new List<Category> { new Category() { CategoryName = "Helloworld" } });

            var result = await _controller.GetAllCategory();

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllCategory_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.GetAllCategory())
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetAllCategory();

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        // ✅ 2. GetCategoryById
        [Fact]
        public async Task GetCategoryById_ShouldReturnOk_WhenExists()
        {
            var id = Guid.NewGuid();

            _mockService.Setup(s => s.GetCategoryById(id))
                        .ReturnsAsync(new Category { CategoryId = id, CategoryName = "Helloworld" });

            var result = await _controller.GetCategoryById(id);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.GetCategoryById(It.IsAny<Guid>()))
                        .ThrowsAsync(new NotFoundException("Not found"));

            var result = await _controller.GetCategoryById(Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        // ✅ 3. GetCategoryByName
        [Fact]
        public async Task GetCategoryByName_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetCategoryByName("Fiction"))
                        .ReturnsAsync(new Category {CategoryName = "Helloworld" });

            var result = await _controller.GetCategoryByName("Fiction");

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCategoryByName_ShouldReturnNotFound()
        {
            _mockService.Setup(s => s.GetCategoryByName(It.IsAny<string>()))
                        .ThrowsAsync(new NotFoundException("Not found"));

            var result = await _controller.GetCategoryByName("Unknown");

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        // ✅ 4. CreateCategory
        [Fact]
        public async Task CreateCategory_ShouldReturnCreated_WhenSuccess()
        {
            var dto = new CreateCategoryDto
            {
                CategoryName = "Fiction"
            };

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Fiction"
            };

            _mockService.Setup(s => s.CreateCategory(dto))
                        .ReturnsAsync(category);

            var result = await _controller.CreateCategory(dto);

            var created = result as CreatedAtActionResult;

            created.Should().NotBeNull();
            created!.StatusCode.Should().Be(201);
            created.ActionName.Should().Be(nameof(CategoryController.GetCategoryById));
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnConflict_WhenDuplicate()
        {
            _mockService.Setup(s => s.CreateCategory(It.IsAny<CreateCategoryDto>()))
                        .ThrowsAsync(new ConflictException("Exists"));

            var result = await _controller.CreateCategory(new CreateCategoryDto() { CategoryName="Hello"});

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.CreateCategory(It.IsAny<CreateCategoryDto>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.CreateCategory(new CreateCategoryDto() { CategoryName = "Hello" });

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
