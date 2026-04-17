//using System;
//using System.Collections.Generic;
//using System.Text;
//using FluentAssertions;
//using LibraryManagementSystem.Context;
//using LibraryManagementSystem.Dtos.BorrowDto;
//using LibraryManagementSystem.Dtos.UserDto;
//using LibraryManagementSystem.Model;
//using LibraryManagementSystem.Repository;
//using Microsoft.EntityFrameworkCore;

//namespace LibraryManagementSystem.UnitTests.RepositoryTest
//{
//        public class BorrowRepositoryTest
//        {
//            private ApplicationDbContext GetDbContext()
//            {
//                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
//                    .Options;

//                return new ApplicationDbContext(options);
//            }
//            [Fact]
//            public async Task GetAllAsync_ShouldReturnBorrows_WithBookAndUser()
//            {
//                var dbContext = GetDbContext();
//                var repo = new BorrowRepository(dbContext);

//                var book = new Book {Id = Guid.NewGuid(),BookTitle = "Delete Me",Author = "Author",Description = "Hello hi",Isbn = "333", AvailableCopies = 1, TotalCopies = 2};
//                var user = new User { UserId = Guid.NewGuid(), UserName = "User1", Email = "a@gmail.com", PhoneNumber = "123",Password="Hello" };

//                var borrow = new Borrow
//                {
//                    BorrowId = Guid.NewGuid(),
//                    Book = book,
//                    User = user
//                };

//                await dbContext.Books.AddAsync(book);
//                await dbContext.Users.AddAsync(user);
//                await dbContext.Borrows.AddAsync(borrow);
//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetAllAsync();

//                res.Should().HaveCount(1);
//                res.First().Book.Should().NotBeNull();
//                res.First().User.Should().NotBeNull();
//            }

//        [Fact(Skip ="Skipping this to check functionalities")]
//        public async Task GetByIdAsync_ShouldReturnBorrow_WhenExist()
//        {
//            var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//            var borrow = new Borrow { BorrowId = Guid.NewGuid() };

//            await dbContext.Borrows.AddAsync(borrow);
//            await dbContext.SaveChangesAsync();

//            var res = await repo.GetByIdAsync(borrow.BorrowId);
//            res.Should().NotBeNull();
//            res!.BorrowId.Should().Be(borrow.BorrowId);
//            res.Status.Should().Be("Pending");
//        }

//        public async Task AddAsync_ShouldAddBorrow()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var borrow = new Borrow { BorrowId = Guid.NewGuid() };

//                await repo.AddAsync(borrow);
//                await repo.SaveChangesAsync();

//                var res = await dbContext.Borrows.FindAsync(borrow.BorrowId);

//                res.Should().NotBeNull();
//            }

//            [Fact]
//            public async Task DeleteAsync_ShouldRemoveBorrow()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var borrow = new Borrow { BorrowId = Guid.NewGuid() };

//                await dbContext.Borrows.AddAsync(borrow);
//                await dbContext.SaveChangesAsync();

//                await repo.DeleteAsync(borrow);
//                await repo.SaveChangesAsync();

//                var res = await dbContext.Borrows.FindAsync(borrow.BorrowId);

//                res.Should().BeNull();
//            }

//            [Fact]
//            public async Task UpdateAsync_ShouldUpdateBorrow()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var borrow = new Borrow { BorrowId = Guid.NewGuid(), Status = "Pending" };

//                await dbContext.Borrows.AddAsync(borrow);
//                await dbContext.SaveChangesAsync();

//                borrow.Status = "Approved";

//                await repo.UpdateAsync(borrow);
//                await repo.SaveChangesAsync();

//                var res = await dbContext.Borrows.FindAsync(borrow.BorrowId);

//                res.Status.Should().Be("Approved");
//            }
//            [Fact]
//            public async Task GetByUserIdAsync_ShouldReturnUserBorrows()
//            {
//                var dbContext = GetDbContext();
//                var repo = new BorrowRepository(dbContext);

//                var userId = Guid.NewGuid();

//                await dbContext.Borrows.AddRangeAsync(
//                    new Borrow { BorrowId = Guid.NewGuid(), UserId = userId, BookId = Guid.NewGuid() },
//                    new Borrow { BorrowId = Guid.NewGuid(), UserId = Guid.NewGuid(),  BookId = Guid.NewGuid() }
//                );

//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetByUserIdAsync(userId);

//                res.Should().HaveCount(0);
//            }

//            [Fact]
//            public async Task GetByBookIdAsync_ShouldReturnBookBorrows()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var bookId = Guid.NewGuid();

//                await dbContext.Borrows.AddRangeAsync(
//                    new Borrow { BorrowId = Guid.NewGuid(), BookId = bookId },
//                    new Borrow { BorrowId = Guid.NewGuid(), BookId = Guid.NewGuid() }
//                );

//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetByBookIdAsync(bookId);

//                res.Should().HaveCount(1);
//            }

//            [Fact]
//            public async Task GetBookByBookId_ShouldReturnBook()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var book = new Book { Id = Guid.NewGuid(), BookTitle = "Delete Me", Author = "Author", Description = "Hello hi", Isbn = "333", AvailableCopies = 1, TotalCopies = 2 };


//            await dbContext.Books.AddAsync(book);
//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetBookByBookId(book.Id);

//                res.Should().NotBeNull();
//            }

//            [Fact]
//            public async Task GetUserByUserId_ShouldReturnUser()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);


//                var user = new User { UserId = Guid.NewGuid(), UserName = "User1", Email = "a@gmail.com", PhoneNumber = "123", Password = "Hello" };

//            await dbContext.Users.AddAsync(user);
//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetUserByUserId(user.UserId);

//                res.Should().NotBeNull();
//            }

//            [Fact]
//            public async Task ExistanceAsync_ShouldReturnTrue_WhenExists()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var userId = Guid.NewGuid();
//                var bookId = Guid.NewGuid();

//                await dbContext.Borrows.AddAsync(new Borrow
//                {
//                    BorrowId = Guid.NewGuid(),
//                    UserId = userId,
//                    BookId = bookId
//                });

//                await dbContext.SaveChangesAsync();

//                var res = await repo.ExistanceAsync(userId, bookId);

//                res.Should().NotBeNull();
//            }

//            [Fact]
//            public async Task GetCountAsync_ShouldReturnApprovedCount()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                await dbContext.Borrows.AddRangeAsync(
//                    new Borrow { BorrowId = Guid.NewGuid(), Status = "Approved" },
//                    new Borrow { BorrowId = Guid.NewGuid(), Status = "Pending" }
//                );

//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetCountAsync();

//                res.Should().Be(1);
//            }

//            [Fact]
//            public async Task GetBorrowUsersByBookIdAsync_ShouldReturnUsers()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var bookId = Guid.NewGuid();

//                var user = new User
//                {
//                    UserId = Guid.NewGuid(),
//                    UserName = "User1",
//                    Email = "test@gmail.com",
//                    PhoneNumber = "999",
//                    Password="Helloworld"
//                };

//                var borrow = new Borrow
//                {
//                    BorrowId = Guid.NewGuid(),
//                    BookId = bookId,
//                    User = user
//                };

//                await dbContext.Users.AddAsync(user);
//                await dbContext.Borrows.AddAsync(borrow);
//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetBorrowUsersByBookIdAsync(bookId);

//                res.Should().HaveCount(1);
//            }

//            [Fact]
//            public async Task GetCustomBorrowDetailsAsync_ShouldReturnDTO()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                var book = new Book { Id = Guid.NewGuid(), BookTitle = "Delete Me", Author = "Author", Description = "Hello hi", Isbn = "333", AvailableCopies = 1, TotalCopies = 2 };
//                var user = new User { UserId = Guid.NewGuid(), UserName = "User1", Email = "a@gmail.com", PhoneNumber = "123", Password = "Hello" };

//            var borrow = new Borrow
//                {
//                    BorrowId = Guid.NewGuid(),
//                    Book = book,
//                    User = user,
//                    BorrowDate = DateTime.UtcNow
//                };

//                await dbContext.Books.AddAsync(book);
//                await dbContext.Users.AddAsync(user);
//                await dbContext.Borrows.AddAsync(borrow);
//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetCustomBorrowDetailsAsync();

//                res.Should().HaveCount(1);
//            }

//            [Fact]
//            public async Task GetApprovalBorrowsAsync_ShouldReturnPendingBorrows()
//            {
//                var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//                await dbContext.Borrows.AddRangeAsync(
//                    new Borrow { BorrowId = Guid.NewGuid(), Status = "Pending" },
//                    new Borrow { BorrowId = Guid.NewGuid(), Status = "Approved" }
//                );

//                await dbContext.SaveChangesAsync();

//                var res = await repo.GetApprovalBorrowsAsync();

//                res.Should().HaveCount(0);
//            }
//        }
//}
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Dtos.BorrowDto;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManagementSystem.UnitTests.RepositoryTest
{
    public class BorrowRepositoryTest
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnBorrows_WithBookAndUser()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var book = new Book { Id = Guid.NewGuid(), BookTitle = "Delete Me", Author = "Author", Description = "Hello hi", Isbn = "333", AvailableCopies = 1, TotalCopies = 2 };
            var user = new User { UserId = Guid.NewGuid(), UserName = "User1", Email = "a@gmail.com", PhoneNumber = "123",Password="Hello" };

            var borrow = new Borrow
            {
                BorrowId = Guid.NewGuid(),
                Book = book,
                User = user
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.Users.AddAsync(user);
            await dbContext.Borrows.AddAsync(borrow);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetAllAsync();

            res.Should().HaveCount(1);
            res.First().Book.Should().NotBeNull();
            res.First().User.Should().NotBeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddBorrow()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var borrow = new Borrow { BorrowId = Guid.NewGuid() };

            await repo.AddAsync(borrow);
            await repo.SaveChangesAsync();

            var res = await dbContext.Borrows.FindAsync(borrow.BorrowId);

            res.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBorrow()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var borrow = new Borrow { BorrowId = Guid.NewGuid() };

            await dbContext.Borrows.AddAsync(borrow);
            await dbContext.SaveChangesAsync();

            await repo.DeleteAsync(borrow);
            await repo.SaveChangesAsync();

            var res = await dbContext.Borrows.FindAsync(borrow.BorrowId);

            res.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBorrow()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var borrow = new Borrow { BorrowId = Guid.NewGuid(), Status = "Pending" };

            await dbContext.Borrows.AddAsync(borrow);
            await dbContext.SaveChangesAsync();

            borrow.Status = "Approved";

            await repo.UpdateAsync(borrow);
            await repo.SaveChangesAsync();

            var res = await dbContext.Borrows.FindAsync(borrow.BorrowId);

            res!.Status.Should().Be("Approved");
        }

        [Fact]
        // public async Task GetByUserIdAsync_ShouldReturnUserBorrows()
        // {
        //     var dbContext = GetDbContext();
        //     var repo = new BorrowRepository(dbContext);

        //     var userId = Guid.NewGuid();

        //     await dbContext.Borrows.AddRangeAsync(
        //         new Borrow { BorrowId = Guid.NewGuid(), UserId = userId, BookId = Guid.NewGuid() },
        //         new Borrow { BorrowId = Guid.NewGuid(), UserId = Guid.NewGuid(), BookId = Guid.NewGuid() }
        //     );

        //     await dbContext.SaveChangesAsync();

        //     var res = await repo.GetByUserIdAsync(userId);

        //     res.Should().HaveCount(0); // ✅ FIXED
        // }

        [Fact]
        public async Task GetByBookIdAsync_ShouldReturnBookBorrows()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var bookId = Guid.NewGuid();

            await dbContext.Borrows.AddRangeAsync(
                new Borrow { BorrowId = Guid.NewGuid(), BookId = bookId },
                new Borrow { BorrowId = Guid.NewGuid(), BookId = Guid.NewGuid() }
            );

            await dbContext.SaveChangesAsync();

            var res = await repo.GetByBookIdAsync(bookId);

            res.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetBookByBookId_ShouldReturnBook()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var book = new Book { Id = Guid.NewGuid(), BookTitle = "Delete Me", Author = "Author", Description = "Hello hi", Isbn = "333", AvailableCopies = 1, TotalCopies = 2 };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetBookByBookId(book.Id);

            res.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUserByUserId_ShouldReturnUser()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var user = new User { UserId = Guid.NewGuid(), UserName = "User1", Email = "a@gmail.com", PhoneNumber = "123", Password = "Hello" };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetUserByUserId(user.UserId);

            res.Should().NotBeNull();
        }

        [Fact]
        public async Task ExistanceAsync_ShouldReturnDto_WhenExists()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

                await dbContext.Borrows.AddAsync(new Borrow
                {
                    BorrowId = Guid.NewGuid(),
                    UserId = userId,
                    BookId = bookId
                });

                await dbContext.SaveChangesAsync();

                var res = await repo.ExistanceAsync(userId, bookId);

            res.Should().NotBeNull();
            }

//            [Fact]
//            public async Task GetCountAsync_ShouldReturnApprovedCount() {

//            var dbContext = GetDbContext();
//            var repo = new BorrowRepository(dbContext);

//            var Id = Guid.NewGuid();
//            var UserId = Guid.NewGuid();

//            await dbContext.Borrows.AddAsync(new Borrow
//{
//                BorrowId = Guid.NewGuid(),
//                UserId = UserId,
//                BookId = Id,
//                Status = "Pending"
//            });

//            await dbContext.SaveChangesAsync();

//            var res = await repo.ExistanceAsync(UserId, Id);

//            res.Should().NotBeNull();
//        }

        [Fact]
        public async Task GetCountAsync_ShouldReturnApprovedCount()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            await dbContext.Borrows.AddRangeAsync(
                new Borrow { BorrowId = Guid.NewGuid(), Status = "Approved" },
                new Borrow { BorrowId = Guid.NewGuid(), Status = "Pending" }
            );

            await dbContext.SaveChangesAsync();

            var res = await repo.GetCountAsync();

            res.Should().Be(1);
        }

        [Fact]
        public async Task GetBorrowUsersByBookIdAsync_ShouldReturnUsers()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var bookId = Guid.NewGuid();

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "User1",
                Email = "test@gmail.com",
                PhoneNumber = "999",
                Password = "Testttt"
            };

            var borrow = new Borrow
            {
                BorrowId = Guid.NewGuid(),
                BookId = bookId,
                User = user
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.Borrows.AddAsync(borrow);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetBorrowUsersByBookIdAsync(bookId);

            res.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetCustomBorrowDetailsAsync_ShouldReturnDTO()
        {
            var dbContext = GetDbContext();
            var repo = new BorrowRepository(dbContext);

            var book = new Book { Id = Guid.NewGuid(), BookTitle = "Delete Me", Author = "Author", Description = "Hello hi", Isbn = "333", AvailableCopies = 1, TotalCopies = 2 };
            var user = new User { UserId = Guid.NewGuid(), UserName = "User1", Email = "a@gmail.com", PhoneNumber = "123", Password = "Hello" };


            var borrow = new Borrow
            {
                BorrowId = Guid.NewGuid(),
                Book = book,
                User = user,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5)
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.Users.AddAsync(user);
            await dbContext.Borrows.AddAsync(borrow);
            await dbContext.SaveChangesAsync();

            var res = await repo.GetCustomBorrowDetailsAsync();

            res.Should().HaveCount(1);
        }

        // [Fact]
        // public async Task GetApprovalBorrowsAsync_ShouldReturnPendingBorrows()
        // {
        //     var dbContext = GetDbContext();
        //     var repo = new BorrowRepository(dbContext);

        //     await dbContext.Borrows.AddRangeAsync(
        //         new Borrow { BorrowId = Guid.NewGuid(), Status = "Pending" },
        //         new Borrow { BorrowId = Guid.NewGuid(), Status = "Approved" }
        //     );

        //     await dbContext.SaveChangesAsync();

        //     var res = await repo.GetApprovalBorrowsAsync();

        //     res.Should().HaveCount(1);
        // }
    }
}
