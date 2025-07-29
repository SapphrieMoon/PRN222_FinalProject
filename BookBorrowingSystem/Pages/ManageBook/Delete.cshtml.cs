using BLL.DTOs;
using BLL.Interfaces;
using BookBorrowingSystem.Pages.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace BookBorrowingSystem.Pages.ManageBook
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IHubContext<LibraryHub> _hubContext;

        public DeleteModel(IBookService bookService, IHubContext<LibraryHub> hubContext)
        {
            _bookService = bookService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public BookDTO Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var book = _bookService.GetBookById(id.Value);
            if (book == null)
            {
                return NotFound();
            }

            Book = book;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var book = _bookService.GetBookById(id.Value);
            if (book != null)
            {
                try
                {
                    _bookService.DeleteBook(id.Value);

                    await _hubContext.Clients.All.SendAsync("ReloadBookIndex");
                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting the book. It may be referenced by other records.");
                    Book = book;
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}