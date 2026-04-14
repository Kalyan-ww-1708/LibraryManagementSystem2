    using LibraryManagementSystem.Context;
    using LibraryManagementSystem.Dtos.BorrowDto;
    using LibraryManagementSystem.Dtos.UserDto;
    using LibraryManagementSystem.Model;
    using LibraryManagementSystem.Repository;
    using LibraryManagementSystem.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml;


namespace LibraryManagementSystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        public BorrowService(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }
        public async Task<List<BorrowDetailsDto>> GetBorrowList()
        {
            var borrowList = await _borrowRepository.GetAllAsync();
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();
        }
        public async Task<List<BorrowDetailsDto>> GetLimitedBorrowsAsync(int limit)
        {
            var borrowList = await _borrowRepository.GetLimitedAsync(limit);
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();
        }
        public async Task<Borrow> GetBorrowWithId(Guid borrowId)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            return borrow;
        }
        public async Task<List<BorrowDetailsDto>> GetBorrowListByUserId(Guid userId)
        {

            var borrowList = await _borrowRepository.GetByUserIdAsync(userId);
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();
        }
        public async Task<List<BorrowDetailsDto>> GetBorrowListByBookId(Guid bookId)
        {
            var borrowList = await _borrowRepository.GetByBookIdAsync(bookId);
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();
        }
        public async Task<List<BorrowDetailsDto>> GetRecentBorrow()
        {
            var borrowList = await _borrowRepository.GetRecentList();
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();
        }

        public async Task<Borrow> CreateBorrow(CreateBorrowDto dto)
        {
            var book = await _borrowRepository.GetBookByBookId(dto.BookId);
            //var book = await _dbContext.Books.FindAsync(dto.BookId);
            if (book == null)
                throw new NotFoundException("Book not found");
            var user = await _borrowRepository.GetUserByUserId(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found");
            if (book.AvailableCopies <= 0)
                throw new InValidException("Book is currently unavailable");
            var exist = await _borrowRepository.ExistanceAsync(dto.UserId, dto.BookId);
            if (exist)
                throw new InValidException("Multiple books can't be taken");

            var borrow = new Borrow
            {
                BookId = dto.BookId,
                UserId = dto.UserId,
                BorrowDate = dto.BorrowDate,
                DueDate = dto.DueDate,
                Book = book,
                User = user,
            };
   
            await _borrowRepository.AddAsync(borrow);
            await _borrowRepository.SaveChangesAsync();
            return borrow;
        }
        public async Task<string> ApproveBorrowRequest(Guid borrowId)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Request Not found");
            if (borrow.Status != "Pending")
                throw new InValidException("Only pending requests can be approved");
            if (borrow.Book?.AvailableCopies <= 0)
                throw new InValidException("No copies available");
            borrow.Status = "Approved";
            borrow.StatusUpdatedAt = DateTime.UtcNow;
            borrow.Book!.AvailableCopies--;
            await _borrowRepository.SaveChangesAsync();
            return borrow.Status;
        }
        public async Task<List<BorrowDetailsDto>> BorrowListForApprovals()
        {
            var borrowList = await _borrowRepository.GetApprovalBorrowsAsync();
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();

        }

        public async Task<string> RejectBorrowRequest(Guid borrowId)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Request Not found");
            if (borrow.Status != "Pending")
                throw new InValidException("Only pending requests can be approved");
            borrow.Status = "Rejected";
            borrow.StatusUpdatedAt = DateTime.UtcNow;
            await _borrowRepository.SaveChangesAsync();
            return borrow.Status;
        }
        public async Task<string> ApproveReturnRequest(Guid borrowId)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Request Not found");
            if (borrow.Status == "Returned" || borrow.Status == "Pending" || borrow.Status == "Rejected")
                throw new InValidException("Invalid Borrow Please Try Again");
            borrow.Status = "Returned";
            borrow.StatusUpdatedAt = DateTime.UtcNow;
            borrow.Book!.AvailableCopies++;
            await _borrowRepository.SaveChangesAsync();
            return "Successfully Returned";
        }
        public async Task<Borrow?> AddReturnDate(Guid borrowId, UpdateBorrowDto dto)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Book not found");
            if (borrow.ReturnDate != null)
                throw new Exception("Book already returned");
            borrow.ReturnDate = dto.ReturnDate;
            borrow.Status = "Return Raised";
            await _borrowRepository.SaveChangesAsync();
            return borrow;
        }
        public async Task<Borrow?> ExtendDueDate(Guid borrowId, UpdateBorrowDto dto)
        {
            var borrow = await _borrowRepository.GetByIdAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Borrow Book not Found");
            if (dto.DueDate.HasValue)
                borrow.DueDate = dto.DueDate.Value;
            await _borrowRepository.SaveChangesAsync();
            return borrow;
        }
        public async Task<List<BorrowDetailsDto>> GetReturnBorrowsAsync()
        {
            var borrowList = await _borrowRepository.GetReturnBorrowsAsync();
            return borrowList.Select(b => new BorrowDetailsDto
            {
                BorrowId = b.BorrowId,
                UserName = b.User != null ? b.User.UserName : "Unknown",
                Email = b.User != null ? b.User.Email : null,
                BookTitle = b.Book != null ? b.Book.BookTitle : "Unknown",
                Status = b.Status,
                StatusUpdatedAt = b.StatusUpdatedAt
            }).ToList();
        }


        public async Task<byte[]> DownloadBorrowList()
        {
            var borrowList = await _borrowRepository.GetAllAsync();

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("BorrowList");

            sheet.Cells[1, 1].Value = "BorrowId";
            sheet.Cells[1, 2].Value = "BookTitle";
            sheet.Cells[1, 3].Value = "UserName";
            sheet.Cells[1, 4].Value = "BorrowDate";
            sheet.Cells[1, 5].Value = "DueDate";
            sheet.Cells[1, 6].Value = "ReturnDate";
            sheet.Cells[1, 7].Value = "PhoneNumber";

            int row = 2;

            foreach (var b in borrowList)
            {
                sheet.Cells[row, 1].Value = b.BorrowId;
                sheet.Cells[row, 2].Value = b.Book != null ? b.Book.BookTitle : "Unknown";
                sheet.Cells[row, 3].Value = b.User != null ? b.User.UserName : "Unknown";
                sheet.Cells[row, 4].Value = b.BorrowDate;
                sheet.Cells[row, 4].Style.Numberformat.Format = "yyyy-mm-dd";

                sheet.Cells[row, 5].Value = b.DueDate;
                sheet.Cells[row, 5].Style.Numberformat.Format = "yyyy-mm-dd";

                sheet.Cells[row, 6].Value = b.ReturnDate ?? (object)"";
                sheet.Cells[row, 6].Style.Numberformat.Format = "yyyy-mm-dd";

                sheet.Cells[row, 7].Value = b.User != null ? b.User.PhoneNumber : "Unknow";

                row++;
            }
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }
        public async Task<int> GetBorrowCount()
        {
            return await _borrowRepository.GetCountAsync();
        }


    }
}
