using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int Quantity { get; set; }
        public int Available { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<BorrowRequest>? BorrowRequests { get; set; }
    }
}
