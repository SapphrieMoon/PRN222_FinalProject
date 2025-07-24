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
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IList<BookDTO> Book { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Book = _bookService.GetAllBooks();
        }
    }
}