using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.BookDto
{
    public class CreateBookDto
    {
        [Required(ErrorMessage ="Book Title can't be empty ")]
        public required string BookTitle { get; set; }

        public required string Author { get; set; } = "Unavailable";

        public required string Description { get; set; } = "Unavaiable";

        public required string Isbn { get; set; } = "ISBN is Unavaiable";

        [Required(ErrorMessage = "Available copies can't be empty")]
        public required int AvailableCopies { get; set; }

        [Required(ErrorMessage = "Total copies can't be empty")]
        public required int TotalCopies { get; set; }

        public string ImageUrl { get; set; } = "Unavaiable";

    }
}
;