using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<List<Borrow>> GetBorrowList();
        Task<List<UserLoginResponseDto>> GetBorrowListByBookId(Guid bookId);
        Task<Borrow> CreateBorrow(CreateBorrowDto dto);

        Task<Borrow?> AddReturnDate(Guid borrowId,UpdateBorrowDto dto);
        Task<Borrow?> ExtendDueDate(Guid borrowId, UpdateBorrowDto dto);
        Task<List<Borrow>> GetBorrowListByUserId(Guid  UserId);

        Task<string> ApproveBorrowRequest(Guid BorrowId);
        Task<string> RejectBorrowRequest(Guid BorrowId);

        Task<byte[]> DownloadBorrowList();
        Task<int> GetBorrowCount();
        Task<List<Borrow>> BorrowListForApprovals();

    }
}
