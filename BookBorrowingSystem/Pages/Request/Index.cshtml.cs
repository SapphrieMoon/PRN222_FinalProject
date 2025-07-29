using BLL.DTOs;
using BLL.Interfaces;
using BookBorrowingSystem.Pages.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace BookBorrowingSystem.Pages.Request
{
    [Authorize(Roles = "Librarian")]
    public class IndexModel : PageModel
    {
        private readonly IRequestService _service;
        private readonly IBookService _bookService;
        private readonly IAccountService _accountSerice;
        private readonly IHubContext<LibraryHub> _hubContext;
        private const int PageSize = 10;

        public IndexModel(IRequestService service, IBookService bookService, IAccountService accountSerice, IHubContext<LibraryHub> hubContext)
        {
            _service = service;
            _bookService = bookService;
            _accountSerice = accountSerice;
            _hubContext = hubContext;
        }

        public List<RequestDTO> Requests { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public List<AccountResDTO> Accounts { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchRequestId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchUserName { get; set; }

        public void OnGet(int pageNumber = 1)
        {
            var allRequests = _service.GetAllRequests(SearchRequestId, SearchUserName);
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

        public async Task<IActionResult> OnPostReturnAsync(int id)
        {
            var request = _service.GetRequestById(id);
            if (request == null)
                return NotFound();

            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
                return Unauthorized();

            request.Status = "Returned";
            request.ProcessedById = int.Parse(accountIdClaim.Value);

            // Lấy Book và tăng Available
            var book = _bookService.GetBookById(request.BookId);
            if (book != null)
            {
                book.Avaliable += 1;
                _bookService.UpdateBook(book);
            }

            _service.UpdateRequest(request);
            await _hubContext.Clients.All.SendAsync("ReloadBookIndex");
            return RedirectToPage(new { pageNumber = PageNumber, SearchRequestId, SearchUserName });
        }
    }
}
