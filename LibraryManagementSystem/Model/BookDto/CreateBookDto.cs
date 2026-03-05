namespace LibraryManagementSystem.Model.BookDto
{
    public class CreateBookDto
    {
        public required string BookTitle { get; set; }
        public required string Author { get; set; }
        public required string Description { get; set; }
        public required string Isbn { get; set; }
        public required int AvailableCopies { get; set; }
        public required int TotalCopies { get; set; }
    }
}
