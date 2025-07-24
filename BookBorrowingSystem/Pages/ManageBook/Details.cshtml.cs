using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Interfaces;
using BLL.DTOs;

namespace BookBorrowingSystem.Pages.ManageBook
{
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