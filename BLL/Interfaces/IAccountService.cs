using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        AccountResDTO Login(string username, string password);
    }
}
