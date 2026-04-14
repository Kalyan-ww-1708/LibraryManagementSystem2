using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.BorrowDto
{
    public class CreateBorrowDto
    {
        [Required(ErrorMessage = "Can't make borrow request without a BookId")]
        public Guid BookId { get; set; }
        [Required(ErrorMessage = "Can't make borrow request without a UserId")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Can't make borrow request without a BorroDate")]
        public DateTime BorrowDate { get; set; }
        [Required(ErrorMessage = "Can't make borrow request without a DueDate")]
        public DateTime DueDate { get; set; }
    }
}
