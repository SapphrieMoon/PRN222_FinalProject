using BLL.Interfaces;
using BLL.DTOs;
using DAL.Interfaces;
using DAL.Repositories;
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

        
        public async Task<AccountResDTO> GetById(int id)
        {
            var account = await _repo.GetById(id);
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

        public async Task CreateAccountAsync(CreateAccountDTO dto)
        {
            var account = new Account
            {
                Username = dto.UserName,
                Email = dto.Email,
                Password = dto.Password, // Lưu plain text (chỉ dùng cho học tập)
                Role = dto.Role
            };
            await _repo.Add(account);
        }

        public async Task UpdateAccountAsync(UpdateAccountDTO dto)
        {
   
            var account = await _repo.GetById(dto.AccountId);
            if (account == null)
                throw new KeyNotFoundException("Account not found.");

            account.Username = dto.UserName ?? account.Username;
            account.Email = dto.Email ?? account.Email;
            account.Password = dto.Password ?? account.Password;
            account.Role = dto.Role ?? account.Role;

            await _repo.Update(account);
        }

        public async Task Delete(int id)
        {
            var account = await _repo.GetById(id);
            if (account == null) throw new KeyNotFoundException("Account not found.");

            await _repo.Delete(id);
        }

        public async Task<IEnumerable<AccountResDTO>> GetAllAccountsAsync()
        {
            var accounts = await _repo.GetAll();
            return accounts.Select(a => new AccountResDTO
            {
                AccountId = a.AccountId,
                UserName = a.Username,
                Email = a.Email,
                Role = a.Role
            });
        }

        public async Task<AccountResDTO?> GetAccountByIdAsync(int id)
        {
            var account = await _repo.GetById(id);
            return account == null ? null : new AccountResDTO
            {
                AccountId = account.AccountId,
                UserName = account.Username,
                Email = account.Email,
                Role = account.Role
            };
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _repo.GetById(id);
            if (account == null) throw new KeyNotFoundException("Account not found.");

            await _repo.Delete(id);
        }
    }

}

