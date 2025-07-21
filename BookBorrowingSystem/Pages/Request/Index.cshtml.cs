using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.Request
{
    public class IndexModel : PageModel
    {
        private readonly IRequestService _service;
        private const int PageSize = 10;

        public IndexModel(IRequestService service)
        {
            _service = service;
        }

        public List<RequestDTO> Requests { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        

        public void OnGet(int pageNumber = 1)
        {
            var allRequests = _service.GetAllRequests();
            int totalRequests = allRequests.Count;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalRequests / (double)PageSize);
            Requests = allRequests
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public IActionResult OnPostBorrow(int id)
        {
            var request = _service.GetRequestById(id);
            if (request == null)
                return NotFound();

            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
                return Unauthorized();

            request.Status = "Borrowed";
            request.ProcessedById = int.Parse(accountIdClaim.Value);

            _service.UpdateRequest(request);

            return RedirectToPage();
        }

        public IActionResult OnPostReturn(int id)
        {
            var request = _service.GetRequestById(id);
            if (request == null)
                return NotFound();

            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
                return Unauthorized();

            request.Status = "Returned";
            request.ProcessedById = int.Parse(accountIdClaim.Value);

            _service.UpdateRequest(request);

            return RedirectToPage(new { pageNumber = PageNumber });
        }
    }
}
