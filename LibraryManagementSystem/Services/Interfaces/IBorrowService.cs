using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<List<Borrow>> GetBorrowList();
        Task<List<ShowUserDto>> GetBorrowListByBookId(Guid bookId);
        Task<Borrow> CreateBorrow(CreateBorrowDto dto);

        Task<Borrow?> AddReturnDate(Guid borrowId,UpdateBorrowDto dto);
        Task<Borrow?> ExtendDueDate(Guid borrowId, UpdateBorrowDto dto);
       
    }
}
