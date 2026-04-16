using FluentAssertions;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryManagementSystem.UnitTests.ControllerTest
{
    public class BorrowControllerTest
    {
        private readonly Mock<IBorrowService> _mockService;
        private readonly BorrowController _controller;

        public BorrowControllerTest()
        {
            _mockService = new Mock<IBorrowService>();
            _controller = new BorrowController(_mockService.Object);
        }
        [Fact]
        public async Task GetBorrowList_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetBorrowList())
                        .ReturnsAsync(new List<BorrowDetailsDto> { new BorrowDetailsDto() });

            var result = await _controller.GetBorrowList();

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetBorrowWithId_ShouldReturnOk_WhenExists()
        {
            var id = Guid.NewGuid();

            _mockService.Setup(s => s.GetBorrowWithId(id))
                        .ReturnsAsync(new Borrow());

            var result = await _controller.GetBorrowWithId(id);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetBorrowWithId_ShouldReturnNotFound_WhenNull()
        {
            _mockService.Setup(s => s.GetBorrowWithId(It.IsAny<Guid>()))
                        .ReturnsAsync((Borrow?)null);

            var result = await _controller.GetBorrowWithId(Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetBorrowListByBookId_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetBorrowListByBookId(It.IsAny<Guid>()))
                        .ReturnsAsync(new List<BorrowDetailsDto> { new BorrowDetailsDto() });

            var result = await _controller.GetBorrowListByBookId(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetBorrowListByUserId_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetBorrowListByUserId(It.IsAny<Guid>()))
                .ReturnsAsync(new List<BorrowDetailsDto> { new BorrowDetailsDto() });


            var result = await _controller.GetBorrowListByUserId(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetBorrowCount_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetBorrowCount())
                        .ReturnsAsync(5);

            var result = await _controller.GetBooksCount();

            var ok = result as OkObjectResult;
            ok!.Value.Should().Be(5);
        }

        [Fact]
        public async Task CreateBorrow_ShouldReturnOk_WhenSuccess()
        {
            var dto = new CreateBorrowDto();

            _mockService.Setup(s => s.CreateBorrow(dto))
                        .ReturnsAsync(new Borrow());

            var result = await _controller.CreateBorrow(dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateBorrow_ShouldReturnNotFound_WhenException()
        {
            _mockService.Setup(s => s.CreateBorrow(It.IsAny<CreateBorrowDto>()))
                        .ThrowsAsync(new NotFoundException("Not Found"));

            var result = await _controller.CreateBorrow(new CreateBorrowDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AddReturnDate_ShouldReturnOk()
        {
            _mockService.Setup(s => s.AddReturnDate(It.IsAny<Guid>(), It.IsAny<UpdateBorrowDto>()))
                        .ReturnsAsync(new Borrow());

            var result = await _controller.AddReturnDate(Guid.NewGuid(), new UpdateBorrowDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ApproveReturnRequest_ShouldReturnOk()
        {
            _mockService.Setup(s => s.ApproveReturnRequest(It.IsAny<Guid>()))
                        .ReturnsAsync("Success");

            var result = await _controller.ApproveReturnRequest(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ApproveReturnRequest_ShouldReturnConflict()
        {
            _mockService.Setup(s => s.ApproveReturnRequest(It.IsAny<Guid>()))
                        .ThrowsAsync(new InValidException("Invalid"));

            var result = await _controller.ApproveReturnRequest(Guid.NewGuid());

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task BorrowListForApprovals_ShouldReturnOk()
        {
            _mockService.Setup(s => s.BorrowListForApprovals())
                        .ReturnsAsync(new List<BorrowDetailsDto> { new BorrowDetailsDto() });

            var result = await _controller.BorrowListForApprovals();

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ExtendDueDate_ShouldReturnOk()
        {
            _mockService.Setup(s => s.ExtendDueDate(It.IsAny<Guid>(), It.IsAny<UpdateBorrowDto>()))
                        .ReturnsAsync(new Borrow());

            var result = await _controller.ExtendDueDate(Guid.NewGuid(), new UpdateBorrowDto());

            result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task DownloadBorrows_ShouldReturnFile()
        {
            _mockService.Setup(s => s.DownloadBorrowList())
                        .ReturnsAsync(new byte[] { 1, 2, 3 });

            var result = await _controller.DownloadBorrows();

            result.Should().BeOfType<FileContentResult>();
        }

        [Fact]
        public async Task ApproveBorrowRequest_ShouldReturnOk()
        {
            _mockService.Setup(s => s.ApproveBorrowRequest(It.IsAny<Guid>()))
                        .ReturnsAsync("Approved");

            var result = await _controller.ApproveBorrowRequest(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ApproveBorrowRequest_ShouldReturnConflict()
        {
            _mockService.Setup(s => s.ApproveBorrowRequest(It.IsAny<Guid>()))
                        .ThrowsAsync(new InValidException("Invalid"));

            var result = await _controller.ApproveBorrowRequest(Guid.NewGuid());

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task RejectBorrowRequest_ShouldReturnOk()
        {
            _mockService.Setup(s => s.RejectBorrowRequest(It.IsAny<Guid>()))
                        .ReturnsAsync("Rejected");

            var result = await _controller.RejectBorrowRequest(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}