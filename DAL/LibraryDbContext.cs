using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using DAL.Extensions;

namespace DAL
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ BorrowRequest
            modelBuilder.Entity<BorrowRequest>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRequests)
                .HasForeignKey(br => br.BookId);

            modelBuilder.Entity<BorrowRequest>()
                .HasOne(br => br.Account)
                .WithMany(a => a.BorrowRequests)
                .HasForeignKey(br => br.AccountId);

            modelBuilder.Entity<BorrowRequest>()
                .HasOne(br => br.ProcessedBy)
                .WithMany(a => a.ProcessedRequests)
                .HasForeignKey(br => br.ProcessedById)
                .OnDelete(DeleteBehavior.NoAction); // tránh xóa cascading

            // Gọi hàm seed
            modelBuilder.SeedData();
        }
    }
}
