namespace LibraryManagementSystem.Dtos.BorrowDto
{
    public class BorrowDetailsDto
    {
            public Guid BorrowId { get; set; }
            public string? BookTitle { get; set; }
            public string? UserName { get; set; }
            public string? PhoneNumber { get; set; }
            public DateTime BorrowDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
    }

}
