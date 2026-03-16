using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.AdminDto
{
    public class CreateAdminDto
    {
        [MinLength(1, ErrorMessage = "AdminName cannot be empty")]
        public required string AdminName { get; set; }

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(6,ErrorMessage = "Password should be atleast 6 Character ")]
        public required string Password { get; set; }
    }
}
