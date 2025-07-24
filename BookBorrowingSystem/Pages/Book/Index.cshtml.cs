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

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Books = _service.SearchBooksByName(SearchTerm);
            }
            else
            {
                Books = _service.GetAllBooks();
            }
        }
    }
}