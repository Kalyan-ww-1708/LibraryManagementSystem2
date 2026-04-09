using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.UserDto
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "UserName can't be empty")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(10,ErrorMessage = "PhoneNumber can't be empty")]
        public required string PhoneNumber { get; set; }

        [MinLength(6,ErrorMessage = "Password can't be empty")]
        public required string Password { get; set; }
    }
}
