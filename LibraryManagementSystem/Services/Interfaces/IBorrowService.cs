using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.BorrowDto;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<List<Borrow>> GetBorrowList();
        Task<Borrow> CreateBorrow(CreateBorrowDto dto);
    }
}
