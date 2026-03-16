using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.BorrowDto
{
    public class CreateBorrowDto
    {
        [Required(ErrorMessage = "Can't make borrow request without a BookId")]
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "Can't make borrow request without a UserId")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Borrow Date Can't be empty")]
        public DateTime BorrowDate { get; set; }

        [Required(ErrorMessage = "Due Date Can't be empty")]
        public DateTime DueDate { get; set; }
    }
}
