using FluentAssertions;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Xunit;

namespace LibraryManagementSystem.UnitTests.ControllerTest
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }
        [Fact]
        public async Task GetUsersCount_ShouldReturnOk()
        {
            _mockService.Setup(s => s.GetUsersCount())
                        .ReturnsAsync(10);

            var result = await _controller.GetBooksCount();

            var ok = result as OkObjectResult;

            ok.Should().NotBeNull();
            ok!.Value.Should().Be(10);
        }

        [Fact]
        public async Task GetUsersCount_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.GetUsersCount())
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetBooksCount();

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task LoginUser_ShouldReturnOk_WhenSuccess()
        {
            var dto = new GetUserDto { Identifier = "user@gmail.com", Password = "123" };

            _mockService.Setup(s => s.LoginUser(dto))
                        .ReturnsAsync(new UserLoginResponseDto
                        {
                            Token = "token",
                            UserName = "user.UserName",
                            Email =" user.Email"
                        });

            var result = await _controller.LoginUser(dto);

            result.Should().BeOfType<OkObjectResult>();
        }


        [Fact(Skip = "Skipping this to check functionalities")]
        public async Task LoginUser_ShouldReturnNotFound_WhenUserNotFound()
        {
            _mockService.Setup(s => s.LoginUser(It.IsAny<GetUserDto>()))
                        .ThrowsAsync(new NotFoundException("Not found"));

            var result = await _controller.LoginUser(new GetUserDto() { Identifier="",Password=""});

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact(Skip = "Skipping this to check functionalities")]
        public async Task LoginUser_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.LoginUser(It.IsAny<GetUserDto>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.LoginUser(new GetUserDto() { Identifier = "k", Password = "k" });

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task RegisterUser_ShouldReturnOk_WhenSuccess()
        {
            var dto = new CreateUserDto {UserName="Test",PhoneNumber="9999999999", Email = "test@gmail.com", Password = "123" };

            _mockService.Setup(s => s.RegisterUser(dto))
                        .ReturnsAsync("Success Otp Sent");

            var result = await _controller.RegisterUser(dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnConflict_WhenExists()
        {
            _mockService.Setup(s => s.RegisterUser(It.IsAny<CreateUserDto>()))
                        .ThrowsAsync(new ConflictException("Exists"));

            var result = await _controller.RegisterUser(new CreateUserDto() { UserName = "Test", PhoneNumber = "9999999999", Email = "test@gmail.com", Password = "123" });

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.RegisterUser(It.IsAny<CreateUserDto>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.RegisterUser(new CreateUserDto() { UserName = "Test", PhoneNumber = "9999999999", Email = "test@gmail.com", Password = "123" });

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task VerifyOtp_ShouldReturnOk_WhenSuccess()
        {
            var dto = new UserVerifyOtpDto { Email = "user@gmail.com", Otp = "123456" };

            _mockService.Setup(s => s.VerifyUser(dto))
                        .ReturnsAsync(new User() { UserName = "Test",Password="Hello", PhoneNumber = "9999999999", Email = "test@gmail.com", });

            var result = await _controller.VerifyOtp(dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task VerifyOtp_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.VerifyUser(It.IsAny<UserVerifyOtpDto>()))
                        .ThrowsAsync(new Exception("Invalid OTP"));

            var result = await _controller.VerifyOtp(new UserVerifyOtpDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}