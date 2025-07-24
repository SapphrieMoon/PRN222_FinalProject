using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _service;

        public IndexModel(IBookService service)
        {
            _service = service;
        }

        [BindProperty]
        public List<BookDTO> Books { get; set; } = new List<BookDTO>();
        public void OnGet()
        {
            Books = _service.GetAllBooks();

        }
    }
}
