using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.Request
{
    public class CreateModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IBookService _bookService;

        public CreateModel(IRequestService requestService, IBookService bookService)
        {
            _requestService = requestService;
            _bookService = bookService;
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
                BorrowDate = DateTime.Now.Date,                    // ✅ Hôm nay
                ReturnDate = DateTime.Now.AddDays(7).Date,         // ✅ Trả sau 7 ngày
                Status = "Pending",
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            // Lấy AccountId từ user đăng nhập
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim != null)
                Request.AccountId = int.Parse(accountIdClaim.Value);

            if (!ModelState.IsValid)
            {
                Book = _bookService.GetBookById(Request.BookId);
                return Page();
            }
            Request.RequestDate = DateTime.Now;
            Request.Status = "Pending";
            Request.ProcessedById = null;

            _requestService.AddRequest(Request);

            return RedirectToPage("/Book/Index");
            // return new JsonResult(new { success = true });

        }
    }
}
