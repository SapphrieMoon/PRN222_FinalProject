using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.Request
{
    [Authorize(Roles = "Librarian")]
    public class DetailsModel : PageModel
    {
        private readonly IRequestService _requestService;

        public DetailsModel(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public RequestDTO Request { get; set; }

        public IActionResult OnGet(int id)
        {
            Request = _requestService.GetRequestById(id);

            if (Request == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}