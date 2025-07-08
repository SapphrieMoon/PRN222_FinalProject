using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private readonly LibraryDbContext _context;

        public AccountRepo(LibraryDbContext context)
        {
            _context = context;
        }

        public Account Login(string email, string password)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email && a.Password == password);
            return account;
        }
    }
}
