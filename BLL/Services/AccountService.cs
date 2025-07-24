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

        public List<AccountResDTO> GetAllAccounts()
        {
            var accounts = _repo.GetAll();
            return accounts.Select(a => new AccountResDTO
            {
                Email = a.Email,
                Password = a.Password,
                UserName = a.Username,
                Role = a.Role,
                //AccountId = a.AccountId
            }).ToList();
        }

        public AccountResDTO GetAccountById(int id)
        {
            var account = _repo.GetById(id);
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

        public void AddAccount(RegisterDTO accountDto)
        {
            var account = new Account
            {
                Username = accountDto.UserName,
                Email = accountDto.Email,
                Password = accountDto.Password,
                Role = "User" // Default role, can be changed later
            };
            _repo.Add(account);
        }

        public void UpdateAccount(AccountResDTO accountDto)
        {
            var account = _repo.GetById(accountDto.AccountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }
            account.Username = accountDto.UserName;
            account.Email = accountDto.Email;
            account.Password = accountDto.Password;
            account.Role = accountDto.Role;
            _repo.Update(account);
        }

        public void DeleteAccount(int accountId)
        {
            _repo.Delete(accountId);
        }
    }
}
