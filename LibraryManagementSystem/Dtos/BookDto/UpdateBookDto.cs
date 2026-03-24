using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Dtos.BookDto
{
    public class UpdateBookDto
    {
        public  string? BookTitle { get; set; }
        public  string? Author { get; set; }
        public  string? Description { get; set; }
        public  string? Isbn { get; set; }
        public  int? AvailableCopies { get; set; }
        public  int? TotalCopies { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? CategoryId { get; set; }
      
    }
}