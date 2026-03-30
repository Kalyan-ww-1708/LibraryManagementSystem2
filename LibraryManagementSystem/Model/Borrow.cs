namespace LibraryManagementSystem.Model
{
    public class Borrow
    {
        public Guid BorrowId { get; set; }
        public Guid BookId { get; set; }

        public Book? Book { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
