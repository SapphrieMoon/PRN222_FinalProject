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
            return _context.Accounts
               .FirstOrDefault(sa => sa.Email == email && sa.Password == password);
        }
        public async Task Add(Account account)
        {
            try
            {
                var existing = await GetById(account.AccountId);
                if (existing != null)
                {
                    throw new Exception("Account with this ID already exist");
                }
                else
                {
                    _context.Accounts.Add(account);
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        

        public async Task<IEnumerable<Account>> GetAll()

        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetById(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.AccountId == id);
        }

        public async Task Update(Account account)
        {
            try
            {
                var existing = await GetById(account.AccountId);
                if (existing == null)
                {
                    throw new Exception("Account not found");
                }
                else
                {
                    existing.Username = account.Username;
                    existing.Password = account.Password;
                    existing.Email = account.Email;
                    existing.Role = account.Role;
               




                    _context.Accounts.Update(existing);
                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                var existing = await GetById(id);
                if (existing == null)
                {
                    throw new Exception("Account not found");
                }
                else
                {
                    _context.Accounts.Remove(existing);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

       
    }
}
