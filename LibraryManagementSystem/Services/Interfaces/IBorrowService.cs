using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<List<BorrowDetailsDto>> GetBorrowList(); //Good
        Task<List<BorrowDetailsDto>> GetLimitedBorrowsAsync(int limit);
        Task<Borrow> GetBorrowWithId(Guid borrowId);
        Task<List<BorrowDetailsDto>> GetBorrowListByBookId(Guid bookId); //Good
        Task<List<BorrowDetailsDto>> GetRecentBorrow();
        Task<Borrow> CreateBorrow(CreateBorrowDto dto);

        Task<Borrow?> AddReturnDate(Guid borrowId,UpdateBorrowDto dto);
        Task<Borrow?> ExtendDueDate(Guid borrowId, UpdateBorrowDto dto);
        Task<List<BorrowDetailsDto>> GetBorrowListByUserId(Guid  UserId);

        Task<string> ApproveBorrowRequest(Guid borrowId);
        Task<string> RejectBorrowRequest(Guid borrowId);
        Task<string> ApproveReturnRequest(Guid borrowId);

        Task<byte[]> DownloadBorrowList();
        Task<int> GetBorrowCount();
        Task<List<BorrowDetailsDto>> BorrowListForApprovals();
        Task<List<BorrowDetailsDto>> GetReturnBorrowsAsync();
    }
}
