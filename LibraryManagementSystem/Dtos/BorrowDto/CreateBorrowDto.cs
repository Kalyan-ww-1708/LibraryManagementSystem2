namespace LibraryManagementSystem.Dtos.BorrowDto
{
    public class CreateBorrowDto
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
