using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.UserDto
{
    public class GetUserDto
    {

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(10, ErrorMessage = "PhoneNumber can't be empty")]
        public string? PhoneNumber { get; set; }

        [MinLength(6, ErrorMessage = "Password can't be empty")]
        public required string Password { get; set; }
    }
}
