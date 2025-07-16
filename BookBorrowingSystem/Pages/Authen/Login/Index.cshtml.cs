using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;


namespace BookBorrowingSystem.Pages.Authen.Login
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _service;

        public IndexModel(IAccountService service)
        {
            _service = service;
        }

        [BindProperty]
        public LoginDTO Account { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var account = _service.Login(Account.Email, Account.Password);
            if (account == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim("AccountId", account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Set to true if you want the user to remain logged in
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Set thex expiration time for the cookie
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

            //Redirect theo Role
            if (account.Role == "Admin")
            {
                return RedirectToPage("/Account/Index");
            }
            if (account.Role == "Librarian")
            {
                return RedirectToPage("/Request/Index");
            }
            if (account.Role == "Student")
            {
                return RedirectToPage("/Book/Index");
            }
            return RedirectToPage("/Index");
        }
    }
}
