using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                new Account { AccountId = 1, Username = "admin", Email = "admin@gmail.com", Password = "123", Role = "Admin" },
                new Account { AccountId = 2, Username = "librarian", Email = "lib@gmail.com", Password = "123", Role = "Librarian" },
                new Account { AccountId = 3, Username = "student", Email = "stu1@gmail.com", Password = "123", Role = "User" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Clean Code", Author = "Robert C. Martin", Quantity = 5, Available = 5, ImagePath = "/images/ThienDuongTungTang.jpg" },
                new Book { BookId = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Quantity = 3, Available = 3, ImagePath = "/images/ChienthuatTungTang.jpg" }
            );

            modelBuilder.Entity<BorrowRequest>().HasData(
                new BorrowRequest
                {
                    RequestId = 1,
                    BookId = 1,
                    AccountId = 3,
                    BorrowDate = DateTime.Today.AddDays(1),
                    ReturnDate = DateTime.Today.AddDays(7),
                    RequestDate = DateTime.Today,
                    Status = "PENDING",
                    ProcessedById = 2 // Giả sử người xử lý là Librarian
                }
            );
        }
    }
}
