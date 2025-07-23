using BLL.Interfaces;
using BLL.DTOs;
using DAL.Interfaces;
using DAL.Entities;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _repo;

        public AccountService(IAccountRepo repo)
        {
            _repo = repo;
        }

        public AccountResDTO Login(string email, string password)
        {
            var account = _repo.Login(email, password);
            if (account == null)
            {
                return null;
            }
            return new AccountResDTO
            {
                Email = account.Email,
                Password = account.Password,
                UserName = account.Username,
                Role = account.Role,
                AccountId = account.AccountId
            };
        }

    }
}
