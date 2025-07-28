using Microsoft.AspNetCore.SignalR;

namespace BookBorrowingSystem.Pages.Hubs
{
    public class LibraryHub : Hub
    {
        public async Task NotifyBorrow(string bookTitle)
        {
            await Clients.All.SendAsync("BookBorrowed", bookTitle);
        }

        public async Task NotifyReturn(string bookTitle)
        {
            await Clients.All.SendAsync("BookReturned", bookTitle);
        }

        // Thêm method mới để thông báo khi có sách mới
        public async Task NotifyBookAdded(string bookTitle, string author)
        {
            await Clients.All.SendAsync("BookAdded", bookTitle, author);
        }

        // Thêm method để refresh toàn bộ danh sách sách
        public async Task RefreshBookList()
        {
            await Clients.All.SendAsync("RefreshBooks");
        }
    }
}