using FluentAssertions;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Dtos.AdminDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Xunit;

namespace LibraryManagementSystem.UnitTests.ControllerTest
{
    public class AdminControllerTest
    {
        private readonly Mock<IAdminService> _mockService;
        private readonly AdminController _controller;

        public AdminControllerTest()
        {
            _mockService = new Mock<IAdminService>();
            _controller = new AdminController(_mockService.Object);
        }


        [Fact]
        public async Task RegisterAdmin_ShouldReturnOk_WhenSuccess()
        {
            var dto = new CreateAdminDto
            {
                Email = "admin@gmail.com",
                AdminName = "TestAdmin",
                Password = "123456"
            };

            _mockService.Setup(s => s.RegisterAdmin(dto)).ReturnsAsync(new Admin{
                AdminId = Guid.NewGuid(),
                AdminName = "TestAdmin",
                Password = "123456",
                Email = "admin@gmail.com",
            });

            var result = await _controller.RegisterAdmin(dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RegisterAdmin_ShouldReturnConflict_WhenAlreadyExists()
        {
            _mockService.Setup(s => s.RegisterAdmin(It.IsAny<CreateAdminDto>()))
                        .ThrowsAsync(new ConflictException("Already exists"));

            var result = await _controller.RegisterAdmin(new CreateAdminDto() { AdminName = "Kalyan", Email = "123@gmail.com", Password = "Helloworld" });

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task RegisterAdmin_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.RegisterAdmin(It.IsAny<CreateAdminDto>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.RegisterAdmin(new CreateAdminDto() { AdminName = "Kalyan", Email = "123@gmail.com", Password = "Helloworld" });

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task LoginAdmin_ShouldReturnOkWithEmail_WhenSuccess()
        {
            var dto = new LoginDto
            {
                Identifier = "admin@gmail.com",
                Password = "123456"
            };

            _mockService.Setup(s => s.LoginAdmin(dto))
                            .ReturnsAsync("123@gmail.com");

            var result = await _controller.LoginAdmin(dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task LoginAdmin_ShouldReturnNotFound_WhenUserNotFound()
        {
            _mockService.Setup(s => s.LoginAdmin(It.IsAny<LoginDto>()))
                        .ThrowsAsync(new NotFoundException("Not found"));

            var result = await _controller.LoginAdmin(new LoginDto()
            {
                Identifier = "admin@gmail.com",
                Password = "123456"
            });

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task LoginAdmin_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.LoginAdmin(It.IsAny<LoginDto>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.LoginAdmin(new LoginDto()
            {
                Identifier = "admin@gmail.com",
                Password = "123456"
            });

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task TfaAdmin_ShouldReturnOk_WhenSuccess()
        {
            var dto = new VerifyOtpDto
            {
                Email = "admin@gmail.com",
                Otp = "123456"
            };

            _mockService.Setup(s => s.TfaAdmin(dto))
                        .ReturnsAsync(new LoginResponseDto
                        {
                            Token = "token",
                            AdminName = "admin.AdminName",
                            Email = "admin.Email"
                        });

            var result = await _controller.TfaAdmin(dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task TfaAdmin_ShouldReturnUnauthorized_WhenOtpInvalid()
        {
            _mockService.Setup(s => s.TfaAdmin(It.IsAny<VerifyOtpDto>()))
                        .ThrowsAsync(new UnauthorizedException("Invalid OTP"));

            var result = await _controller.TfaAdmin(new VerifyOtpDto()
            {
                Email = "admin@gmail.com",
                Otp = "123456"
            });

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }


        [Fact]
        public async Task TfaAdmin_ShouldReturnBadRequest_OnException()
        {
            _mockService.Setup(s => s.TfaAdmin(It.IsAny<VerifyOtpDto>()))
                        .ThrowsAsync(new Exception("Error"));

            var result = await _controller.TfaAdmin(new VerifyOtpDto()
            {
                Email = "admin@gmail.com",
                Otp = "123456"
            });

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}

