using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Dtos.BorrowDto
{
    public class UpdateBorrowDto
    {
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
