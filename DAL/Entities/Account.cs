using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        public ICollection<BorrowRequest>? BorrowRequests { get; set; }
        public ICollection<BorrowRequest>? ProcessedRequests { get; set; }
    }
}
