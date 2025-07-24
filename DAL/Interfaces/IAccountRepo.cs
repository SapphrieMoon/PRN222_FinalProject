using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAccountRepo
    {
        Account Login(string email, string password);
        Task<Account> GetById(int id);
        Task Add(Account account);
        Task Update(Account account);

        Task Delete(int id);

        Task<IEnumerable<Account>> GetAll();

    }
}
