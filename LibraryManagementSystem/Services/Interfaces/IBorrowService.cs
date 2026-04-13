using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<List<Borrow>> GetBorrowList(); //Good
        Task<Borrow> GetBorrowWithId(Guid borrowId);
        Task<List<Borrow>> GetBorrowListByBookId(Guid bookId); //Good
        Task<List<Borrow>> GetRecentBorrow();
        Task<Borrow> CreateBorrow(CreateBorrowDto dto);

        Task<Borrow?> AddReturnDate(Guid borrowId,UpdateBorrowDto dto);
        Task<Borrow?> ExtendDueDate(Guid borrowId, UpdateBorrowDto dto);
        Task<List<Borrow>> GetBorrowListByUserId(Guid  UserId);

        Task<string> ApproveBorrowRequest(Guid borrowId);
        Task<string> RejectBorrowRequest(Guid borrowId);
        Task<string> ApproveReturnRequest(Guid borrowId);

        Task<byte[]> DownloadBorrowList();
        Task<int> GetBorrowCount();
        Task<List<Borrow>> BorrowListForApprovals();
        Task<List<Borrow>> GetReturnBorrowsAsync();



    }
}
