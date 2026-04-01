using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static System.Reflection.Metadata.BlobBuilder;


namespace LibraryManagementSystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly ApplicationDbContext _dbContext;
        public BorrowService(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<List<Borrow>> GetBorrowList()
        {
            var borrows = await _dbContext.Borrows.Include(b => b.Book).Include(b => b.User).ToListAsync();
            return borrows;
        }
        public async Task<List<Borrow>> GetBorrowListByUserId(Guid userId)
        {

            var borrowList = await _dbContext.Borrows.Where(u => u.UserId == userId).ToListAsync();
            return borrowList;
        }
        public async Task<List<UserLoginResponseDto>> GetBorrowListByBookId(Guid bookId)
        {
            var result = await _dbContext.Borrows.Where(b => b.BookId == bookId).Select(b => new UserLoginResponseDto
            {
                UserName = b.User.UserName,
                Email = b.User.Email,
                PhoneNumber = b.User.PhoneNumber
            }).ToListAsync();

            if (!result.Any())
                return null;

            return result;
        }


        public async Task<Borrow> CreateBorrow(CreateBorrowDto dto)
        {
            var book = await _dbContext.Books.FindAsync(dto.BookId);
            if (book == null)
                throw new NotFoundException("Book not found");
            var user = await _dbContext.Users.FindAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found");
            if (book.AvailableCopies <= 0)
                throw new InValidException("Book is currently unavailable");
            var exist = await _dbContext.Borrows.FirstOrDefaultAsync(u => u.UserId == dto.UserId && dto.BookId == u.BookId);
            if (exist != null)
                throw new InValidException("Multiple books can't be taken");

            var borrow = new Borrow{
                BookId = dto.BookId,
                UserId = dto.UserId,
                BorrowDate = dto.BorrowDate,
                DueDate = dto.DueDate,
                Book = book,
                User = user,
            };

            await _dbContext.Borrows.AddAsync(borrow);
            await _dbContext.SaveChangesAsync();
            return borrow;
        }
        public async Task<string> ApproveBorrowRequest(Guid borrowId)
        {
            var borrow = await _dbContext.Borrows.Include(b => b.Book).FirstOrDefaultAsync(b => b.BorrowId == borrowId);
            if (borrow == null)
                throw new NotFoundException("Request Not found");
            if (borrow.Status != "Pending")
                throw new InValidException("Only pending requests can be approved");
            if (borrow.Book.AvailableCopies <= 0)
                throw new InValidException("No copies available");
            borrow.Status = "Approved";
            borrow.StatusUpdatedAt = DateTime.UtcNow;
            borrow.Book.AvailableCopies--;

            await _dbContext.SaveChangesAsync();
            return borrow.Status;
        }
        public async Task<List<Borrow>> BorrowListForApprovals()
        {
            var borrows = await _dbContext.Borrows.Where(b => b.Status == "Pending").ToListAsync();
            if (!borrows.Any())
                throw new NotFoundException("No Borrows Currently");
            return borrows;
        }
        
        public async Task<string> RejectBorrowRequest(Guid borrowId)
        {
            var borrow = await _dbContext.Borrows.FindAsync(borrowId);
            if(borrow == null)
                throw new NotFoundException("Request Not found");
            if (borrow.Status != "Pending")
                throw new InValidException("Only pending requests can be approved");
            borrow.Status = "Rejected";
            borrow.StatusUpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return borrow.Status;
        }

        public async Task<Borrow?> AddReturnDate(Guid borrowId, UpdateBorrowDto dto)
        {
            var borrow = await _dbContext.Borrows.FindAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Book not found");

            borrow.ReturnDate = dto.ReturnDate;
            borrow.Status = "Returned";
            borrow.StatusUpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return borrow;
        }
        public async Task<Borrow?> ExtendDueDate(Guid borrowId, UpdateBorrowDto dto)
        {
            var borrow = await _dbContext.Borrows.FindAsync(borrowId);
            if (borrow == null)
                throw new NotFoundException("Borrow Book not Found");
            if (dto.DueDate.HasValue)
                borrow.DueDate = dto.DueDate.Value;
            await _dbContext.SaveChangesAsync();
            return borrow;
        }
       
        public async Task<byte[]> DownloadBorrowList()
        {
            var borrowList = await _dbContext.Borrows.Select(b => new { b.BorrowId, BookTitle = b.Book.BookTitle, UserName = b.User.UserName, 
                PhoneNumber = b.User.PhoneNumber,
                b.BorrowDate, b.DueDate, b.ReturnDate}).ToListAsync();

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("BorrowList");

            sheet.Cells[1, 1].Value = "BorrowId";
            sheet.Cells[1, 2].Value = "BookTitle";
            sheet.Cells[1, 3].Value = "UserName";
            sheet.Cells[1, 4].Value = "BorrowDate";
            sheet.Cells[1, 5].Value = "DueDate";
            sheet.Cells[1, 6].Value = "ReturnDate";
            sheet.Cells[1, 7].Value = "PhoneNumber";

            for (int i = 0; i < borrowList.Count; i++){
                var b = borrowList[i];

                sheet.Cells[i + 2, 1].Value = b.BorrowId;
                sheet.Cells[i + 2, 2].Value = b.BookTitle;
                sheet.Cells[i + 2, 3].Value = b.UserName;
                sheet.Cells[i + 2, 4].Value = b.BorrowDate;
                sheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-mm-dd";

                sheet.Cells[i + 2, 5].Value = b.DueDate;
                sheet.Cells[i + 2, 5].Style.Numberformat.Format = "yyyy-mm-dd";

                sheet.Cells[i + 2, 6].Value = b.ReturnDate ?? (object)"";
                sheet.Cells[i + 2, 6].Style.Numberformat.Format = "yyyy-mm-dd";
                sheet.Cells[i + 2, 7].Value = b.PhoneNumber;
            }
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }
        public async Task<int> GetBorrowCount()
        {
            var count = await _dbContext.Borrows.Where(u => u.Status == "Approved").CountAsync();
            return count;
        }
    }


    }
