using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Interfaces;
using BLL.DTOs;

namespace BookBorrowingSystem.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IAccountService _accountService;
        private readonly IRequestService _requestService;

        public IndexModel(IBookService bookService, IAccountService accountService, IRequestService requestService)
        {
            _bookService = bookService;
            _accountService = accountService;
            _requestService = requestService;
        }

        // Filter properties
        [BindProperty]
        public DateTime? FromDate { get; set; }
        [BindProperty]
        public DateTime? ToDate { get; set; }

        // Dashboard Statistics Properties
        public int TotalBooks { get; set; }
        public int TotalAvailableBooks { get; set; }
        public int TotalBorrowedBooks { get; set; }
        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalPendingRequests { get; set; }
        public int TotalApprovedRequests { get; set; }
        public int TotalRequestsToday { get; set; }

        // Lists for detailed views
        public List<BookDTO> RecentBooks { get; set; } = new List<BookDTO>();
        public List<BookDTO> LowStockBooks { get; set; } = new List<BookDTO>();
        public List<RequestDTO> RecentRequests { get; set; } = new List<RequestDTO>();
        public List<RequestDTO> PendingRequests { get; set; } = new List<RequestDTO>();
        public List<AccountResDTO> RecentUsers { get; set; } = new List<AccountResDTO>();

        // Chart data for reports
        public Dictionary<string, int> RequestStatusChart { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MonthlyRequestsChart { get; set; } = new Dictionary<string, int>();
        public List<BookPopularityData> PopularBooksChart { get; set; } = new List<BookPopularityData>();

        public async Task OnGetAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Set default date range if not provided
            FromDate = fromDate ?? DateTime.Now.AddMonths(-1);
            ToDate = toDate ?? DateTime.Now;

            await LoadDashboardData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate date range
            if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
            {
                ModelState.AddModelError("", "From Date cannot be greater than To Date");
                ToDate = DateTime.Now;
                FromDate = DateTime.Now.AddMonths(-1);
            }

            await LoadDashboardData();
            return Page();
        }

        private async Task LoadDashboardData()
        {
            try
            {
                // Load basic statistics
                await LoadBasicStatistics();

                // Load detailed data
                LoadBookData();
                LoadRequestData();
                await LoadUserData();

                // Load chart data
                LoadChartData();
            }
            catch (Exception ex)
            {
                // Log error (you might want to use a proper logging framework)
                TempData["Error"] = "Error loading dashboard data: " + ex.Message;
            }
        }

        private async Task LoadBasicStatistics()
        {
            // Book statistics
            var allBooks = _bookService.GetAllBooks();
            TotalBooks = allBooks.Count;
            TotalAvailableBooks = allBooks.Sum(b => b.Avaliable);
            TotalBorrowedBooks = allBooks.Sum(b => b.Quantity - b.Avaliable);

            // User statistics
            var allAccounts = await _accountService.GetAllAccountsAsync();
            var accountsList = allAccounts.ToList();
            TotalUsers = accountsList.Count(a => a.Role?.ToLower() == "user");
            TotalAdmins = accountsList.Count(a => a.Role?.ToLower() == "admin");

            // Request statistics with date filter
            var allRequests = _requestService.GetAllRequests(null, null);
            var filteredRequests = FilterRequestsByDate(allRequests);

            TotalPendingRequests = filteredRequests.Count(r => r.Status?.ToUpper() == "PENDING");
            TotalApprovedRequests = filteredRequests.Count(r => r.Status?.ToUpper() == "BORROWED");
            TotalRequestsToday = allRequests.Count(r => r.RequestDate.Date == DateTime.Today);
        }

        private void LoadBookData()
        {
            var allBooks = _bookService.GetAllBooks();

            // Recent books (assuming BookId represents creation order)
            RecentBooks = allBooks.OrderByDescending(b => b.BookId).Take(5).ToList();

            // Low stock books (available quantity <= 2)
            LowStockBooks = allBooks.Where(b => b.Avaliable <= 2 && b.Avaliable > 0).Take(10).ToList();
        }

        private void LoadRequestData()
        {
            var allRequests = _requestService.GetAllRequests(null, null);
            var filteredRequests = FilterRequestsByDate(allRequests);

            // Recent requests from filtered data
            RecentRequests = filteredRequests.OrderByDescending(r => r.RequestDate).Take(10).ToList();

            // Pending requests from filtered data
            PendingRequests = filteredRequests.Where(r => r.Status?.ToUpper() == "PENDING")
                                       .OrderByDescending(r => r.RequestDate).Take(10).ToList();
        }

        private async Task LoadUserData()
        {
            var allAccounts = await _accountService.GetAllAccountsAsync();

            // Recent users (assuming AccountId represents creation order)
            RecentUsers = allAccounts.OrderByDescending(a => a.AccountId).Take(5).ToList();
        }

        private void LoadChartData()
        {
            var allRequests = _requestService.GetAllRequests(null, null);
            var filteredRequests = FilterRequestsByDate(allRequests);

            // Request status distribution from filtered data (removed rejected)
            RequestStatusChart = new Dictionary<string, int>
            {
                ["Pending"] = filteredRequests.Count(r => r.Status?.ToUpper() == "PENDING"),
                ["Approved"] = filteredRequests.Count(r => r.Status?.ToUpper() == "BORROWED")
            };

            // Monthly requests within the selected date range
            var monthlyData = filteredRequests
                .GroupBy(r => r.RequestDate.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => g.Count());

            MonthlyRequestsChart = monthlyData;

            // Popular books from filtered requests
            var bookRequests = filteredRequests.GroupBy(r => r.BookId);
            var allBooks = _bookService.GetAllBooks();

            PopularBooksChart = bookRequests
                .Select(g => new BookPopularityData
                {
                    BookTitle = allBooks.FirstOrDefault(b => b.BookId == g.Key)?.Title ?? "Unknown",
                    RequestCount = g.Count()
                })
                .OrderByDescending(b => b.RequestCount)
                .Take(10)
                .ToList();
        }

        private List<RequestDTO> FilterRequestsByDate(List<RequestDTO> requests)
        {
            if (!FromDate.HasValue && !ToDate.HasValue)
                return requests;

            var filtered = requests.AsQueryable();

            if (FromDate.HasValue)
                filtered = filtered.Where(r => r.RequestDate.Date >= FromDate.Value.Date);

            if (ToDate.HasValue)
                filtered = filtered.Where(r => r.RequestDate.Date <= ToDate.Value.Date);

            return filtered.ToList();
        }

        // Action methods for AJAX requests
        public async Task<IActionResult> OnGetRequestStatusDataAsync()
        {
            await LoadBasicStatistics();
            return new JsonResult(RequestStatusChart);
        }

        public async Task<IActionResult> OnGetMonthlyRequestsDataAsync()
        {
            LoadChartData();
            return new JsonResult(MonthlyRequestsChart);
        }

        public async Task<IActionResult> OnGetPopularBooksDataAsync()
        {
            LoadChartData();
            return new JsonResult(PopularBooksChart);
        }
    }

    // Helper class for chart data
    public class BookPopularityData
    {
        public string BookTitle { get; set; } = string.Empty;
        public int RequestCount { get; set; }
    }
}