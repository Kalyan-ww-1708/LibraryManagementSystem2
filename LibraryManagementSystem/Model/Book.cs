using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Model
{
    public class Book
    {
        public Guid Id { get; set; }
        public required string BookTitle { get; set; }
        public required string Author { get; set; }
        public required string Description { get; set; }
        public required string Isbn { get; set; }
        public required int AvailableCopies { get; set; }
        public required int TotalCopies { get; set; }
        public Guid? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public  Category? Categories { get; set; }

        // public string? ImageUrl { get; set; }
    }
}
