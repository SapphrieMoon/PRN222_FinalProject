using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class BorrowRequest
    {
        [Key]
        public int RequestId { get; set; }
        public int BookId { get; set; }
        public int AccountId { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public string Status { get; set; } = "PENDING"; // APPROVED, REJECTED, PENDING
        public int? ProcessedById { get; set; }

        public Book? Book { get; set; }
        public Account? Account { get; set; }
        public Account? ProcessedBy { get; set; }
    }
}
