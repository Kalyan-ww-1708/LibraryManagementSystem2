using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.AdminDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "EMail or Admin Name is required")]
        public required string Identifier { get; set; }

        [MinLength(6, ErrorMessage = "Password Should be atleast 6 characters ")]
        public  required string Password { get; set; }
    }
}
