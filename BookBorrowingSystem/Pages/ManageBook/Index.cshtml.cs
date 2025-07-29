using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.ManageBook
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IList<BookDTO> Book { get; set; } = default!;

        // Property to identify the newest books (highest ID)
        public int? NewestBookId { get; set; }
        public async Task OnGetAsync()
        {
            //Book = _bookService.GetAllBooks();
            var books = _bookService.GetAllBooks();

            // Sort by BookId in descending order to show newest first
            Book = books.OrderByDescending(b => b.BookId).ToList();

            // Set the newest book ID (if there are any books)
            if (Book.Any())
            {
                NewestBookId = Book.First().BookId;
            }
        }
    }
}