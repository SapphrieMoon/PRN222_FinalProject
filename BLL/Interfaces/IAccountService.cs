using BLL.DTOs;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        AccountResDTO Login(string username, string password);

        Task<IEnumerable<AccountResDTO>> GetAllAccountsAsync();
        Task<AccountResDTO?> GetAccountByIdAsync(int id);
        Task CreateAccountAsync(CreateAccountDTO dto);
        Task UpdateAccountAsync(UpdateAccountDTO dto);
        Task DeleteAccountAsync(int id);


    }
}
