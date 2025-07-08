using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookBorrowingSystem.Pages.Request
{
    public class IndexModel : PageModel
    {
        private readonly IRequestService _service;

        public IndexModel(IRequestService service)
        {
            _service = service;
        }

        public List<RequestDTO> Requests { get; set; }

        public void OnGet()
        {
            Requests = _service.GetAllRequests();
        }
    }
}
