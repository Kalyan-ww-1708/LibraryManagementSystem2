using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.BookDto
{
    public class CreateBookDto
    {
        [Required(ErrorMessage ="Book Title can't be empty ")]
        public required string BookTitle { get; set; }

        public required string Author { get; set; } = "Unavailable";

        public required string Description { get; set; } = "Unavaiable";


        [Required(ErrorMessage = "ISBN can't be empty")]
        public required string Isbn { get; set; }

        [Required(ErrorMessage = "Available copies can't be empty")]
        public required int AvailableCopies { get; set; }

        [Required(ErrorMessage = "Total copies can't be empty")]
        public required int TotalCopies { get; set; }
    }
}
