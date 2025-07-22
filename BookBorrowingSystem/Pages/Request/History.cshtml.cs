using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BookBorrowingSystem.Pages.Request
{
    public class HistoryModel : PageModel
    {
        private readonly IRequestService _requestService;
        public List<RequestDTO> UserRequests { get; set; }

        public HistoryModel(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public IActionResult OnGet()
        {
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
            {
                return RedirectToPage("/Authen/Login/Index");
            }
            int accountId = int.Parse(accountIdClaim.Value);
            // Lấy tất cả request của user hiện tại
            UserRequests = _requestService.GetAllRequests(null, null)
                .Where(r => r.AccountId == accountId)
                .OrderByDescending(r => r.RequestDate)
                .ToList();
            return Page();
        }
    }
} 