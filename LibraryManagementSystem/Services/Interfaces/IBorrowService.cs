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

       
    }
}
