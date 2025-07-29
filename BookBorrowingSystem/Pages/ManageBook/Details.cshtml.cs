using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.ManageBook
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IBookService _bookService;

        public DetailsModel(IBookService bookService)
        {
            _bookService = bookService;
        }

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
    }
}