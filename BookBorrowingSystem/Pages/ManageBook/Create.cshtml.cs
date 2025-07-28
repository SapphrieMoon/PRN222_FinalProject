using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using BookBorrowingSystem.Pages.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBorrowingSystem.Pages.ManageBook
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IHubContext<LibraryHub> _hubContext;

        public CreateModel(IBookService bookService, IHubContext<LibraryHub> hubContext)
        {
            _bookService = bookService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public BookDTO Book { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Book == null)
            {
                return Page();
            }

            try
            {
                _bookService.AddBook(Book);

                await _hubContext.Clients.All.SendAsync("ReloadBookIndex");
                // Gửi thông báo real-time khi thêm sách thành công
                //await _hubContext.Clients.All.SendAsync("BookAdded", Book.Title, Book.Author);
                //TempData["SuccessMessage"] = $"Book '{Book.Title}' has been added successfully!";

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the book.");
                return Page();
            }
        }
    }
}