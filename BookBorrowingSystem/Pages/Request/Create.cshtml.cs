using BLL.DTOs;
using BLL.Interfaces;
using BookBorrowingSystem.Pages.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace BookBorrowingSystem.Pages.Request
{
    [Authorize()]
    public class CreateModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IBookService _bookService;
        private readonly IHubContext<LibraryHub> _hubContext;
        public CreateModel(IRequestService requestService, IBookService bookService, IHubContext<LibraryHub> hubContext)
        {
            _requestService = requestService;
            _bookService = bookService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public RequestDTO Request { get; set; }
        public BookDTO Book { get; set; }

        public IActionResult OnGet(int bookId)
        {
            var book = _bookService.GetBookById(bookId);
            if (book == null)
            {
                return NotFound($"Book with ID {bookId} not found.");
            }
            Book = new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Quantity = book.Quantity,
                Avaliable = book.Avaliable,
                ImagePath = book.ImagePath
            };
            Request = new RequestDTO
            {
                BookId = bookId,
                RequestDate = DateTime.Now,
                BorrowDate = DateTime.Now.AddDays(1).Date,                    // ✅ Hôm nay
                ReturnDate = DateTime.Now.AddDays(7).Date,         // ✅ Trả sau 7 ngày
                Status = "Pending",
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy AccountId từ user đăng nhập
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim != null)
                Request.AccountId = int.Parse(accountIdClaim.Value);

            // Kiểm tra logic mượn và trả
            if (Request.ReturnDate <= Request.BorrowDate)
            {
                TempData["ErrorMessage"] = "Return date must be after borrow date.";
                return RedirectToPage("/Book/Index");
            }

            if (!ModelState.IsValid)
            {
                Book = _bookService.GetBookById(Request.BookId);
                return Page();
            }
            Request.RequestDate = DateTime.Now;
            Request.Status = "Pending";
            Request.ProcessedById = null;

            _requestService.AddRequest(Request);
            await _hubContext.Clients.All.SendAsync("ReloadBookIndex");
            var book = _bookService.GetBookById(Request.BookId);
            if (book != null && book.Avaliable > 0)
            {
                book.Avaliable -= 1;
                _bookService.UpdateBook(book);
            }

            return RedirectToPage("/Book/Index");

        }
    }
}
