using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class RequestDTO
    {
        public int RequestId { get; set; }
        public int BookId { get; set; }
        public int AccountId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public int? ProcessedById { get; set;  }
    }
}
