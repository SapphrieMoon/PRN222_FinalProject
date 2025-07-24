using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Interfaces;
using BLL.DTOs;

namespace BookBorrowingSystem.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IEnumerable<AccountResDTO> Accounts { get; set; } = new List<AccountResDTO>();

        [BindProperty]
        public CreateAccountDTO NewAccount { get; set; } = new CreateAccountDTO
        {
            UserName = "",
            Email = "",
            Password = "",
            Role = "User"
        };

        [BindProperty]
        public UpdateAccountDTO EditAccount { get; set; } = new UpdateAccountDTO();

        [TempData]
        public string Message { get; set; } = "";

        [TempData]
        public string MessageType { get; set; } = "";

        public async Task OnGetAsync()
        {
            try
            {
                Accounts = await _accountService.GetAllAccountsAsync();
            }
            catch (Exception ex)
            {
                Message = $"Error loading accounts: {ex.Message}";
                MessageType = "error";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                await _accountService.CreateAccountAsync(NewAccount);
                Message = "Account created successfully!";
                MessageType = "success";

                // Reset form
                NewAccount = new CreateAccountDTO
                {
                    UserName = "",
                    Email = "",
                    Password = "",
                    Role = "User"
                };
            }
            catch (Exception ex)
            {
                Message = $"Error creating account: {ex.Message}";
                MessageType = "error";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                await _accountService.UpdateAccountAsync(EditAccount);
                Message = "Account updated successfully!";
                MessageType = "success";
            }
            catch (Exception ex)
            {
                Message = $"Error updating account: {ex.Message}";
                MessageType = "error";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                Message = "Account deleted successfully!";
                MessageType = "success";
            }
            catch (Exception ex)
            {
                Message = $"Error deleting account: {ex.Message}";
                MessageType = "error";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    Message = "Account not found!";
                    MessageType = "error";
                    return RedirectToPage();
                }

                EditAccount = new UpdateAccountDTO
                {
                    AccountId = account.AccountId,
                    UserName = account.UserName,
                    Email = account.Email,
                    Role = account.Role
                };

                await OnGetAsync();
                return Page();
            }
            catch (Exception ex)
            {
                Message = $"Error loading account: {ex.Message}";
                MessageType = "error";
                return RedirectToPage();
            }
        }
    }
}