using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Repository
{
    public interface IBorrowRepository
    {
        Task<List<Borrow>> GetAllAsync();
        Task<Borrow?> GetByIdAsync(Guid borrowId);

        Task<List<Borrow>> GetRecentList();

        Task AddAsync(Borrow borrow);
        Task UpdateAsync(Borrow borrow);
        Task DeleteAsync(Borrow borrow);
        Task SaveChangesAsync();


        Task<List<Borrow>> GetByUserIdAsync(Guid userId);
        Task<List<Borrow>> GetByBookIdAsync(Guid bookId);
        Task<List<Borrow>> GetLimitedAsync(int limit);

        Task<User?> GetUserByUserId(Guid userId);
        Task<Book?> GetBookByBookId(Guid bookId);

        Task<List<UserLoginResponseDto>> GetBorrowUsersByBookIdAsync(Guid bookId);
        Task<bool> ExistanceAsync(Guid userId, Guid bookId);

        Task<int> GetCountAsync();
        Task<List<BorrowDetailsDto>> GetCustomBorrowDetailsAsync();
        Task<List<Borrow>> GetApprovalBorrowsAsync();

        Task<List<Borrow>> GetReturnBorrowsAsync();


        
      

        
        

    }
}
