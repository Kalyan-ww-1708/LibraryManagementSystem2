//using Xunit;
//using Moq;
//using FluentAssertions;
//using Microsoft.Extensions.Configuration;
//using LibraryManagementSystem.Services;
//using LibraryManagementSystem.Model;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;

//namespace LibraryManagementSystem.UnitTests.ServiceTest
//{
//    public class TokenServiceTest
//    {
//        private readonly Mock<IConfiguration> _mockConfig;
//        private readonly TokenService _tokenService;

//        public TokenServiceTest()
//        {
//            _mockConfig = new Mock<IConfiguration>();

//            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("ThisIsASecretKeyForJwt12345");
//            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
//            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
//            _mockConfig.Setup(c => c["Jwt:ExpiryMinutes"]).Returns("60");

//            _tokenService = new TokenService(_mockConfig.Object);
//        }
//        [Fact]
//        public void GenerateAdminToken_ShouldReturnValidToken_WithCorrectClaims()
//        {
//            var admin = new Admin
//            {
//                AdminId = Guid.NewGuid(),
//                AdminName = "AdminUser",
//                Email = "admin@test.com",
//                Password="Hello"
//            };

//            var token = _tokenService.GenerateAdminToken(admin);

//            token.Should().NotBeNullOrEmpty();

//            var handler = new JwtSecurityTokenHandler();
//            var jwtToken = handler.ReadJwtToken(token);

//            jwtToken.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == "AdminUser");
//            jwtToken.Claims.Should().Contain(c => c.Type == "email" && c.Value == "admin@test.com");
//            jwtToken.Claims.Should().Contain(c => c.Type == "AdminId" && c.Value == admin.AdminId.ToString());
//            jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == "Admin");
//        }

   
//        [Fact]
//        public void GenerateUserToken_ShouldReturnValidToken_WithCorrectClaims()
//        {
//            var user = new User
//            {
//                UserId = Guid.NewGuid(),
//                UserName = "TestUser",
//                Email = "user@test.com",
//                PhoneNumber = "9999999999",
//                Password = "Hello"
//            };

//            var token = _tokenService.GenerateUserToken(user);

//            token.Should().NotBeNullOrEmpty();

//            var handler = new JwtSecurityTokenHandler();
//            var jwtToken = handler.ReadJwtToken(token);

//            jwtToken.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == "TestUser");
//            jwtToken.Claims.Should().Contain(c => c.Type == "email" && c.Value == "user@test.com");
//            jwtToken.Claims.Should().Contain(c => c.Type == "UserId" && c.Value == user.UserId.ToString());
//            jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == "User");
//        }

//        // ✅ 3. Token Should Have Expiry
//        [Fact]
//        public void GenerateToken_ShouldContainExpiry()
//        {
//            var user = new User
//            {
//                UserId = Guid.NewGuid(),
//                UserName = "TestUser",
//                Email = "user@test.com",
//                PhoneNumber = "9999999999",
//                Password = "Hello"
//            };

//            var token = _tokenService.GenerateUserToken(user);

//            var handler = new JwtSecurityTokenHandler();
//            var jwtToken = handler.ReadJwtToken(token);

//            jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
//        }
//    }
//}