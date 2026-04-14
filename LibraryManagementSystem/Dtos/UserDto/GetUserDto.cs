using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.UserDto
{
    public class GetUserDto
    {

        [Required(ErrorMessage = "Please Enter PhoneNumber or Email")]
        public string Identifier { get; set; }

        [MinLength(6, ErrorMessage = "Password can't be empty")]
        public required string Password { get; set; }
    }
}
