using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBookService
    {
        List<BookDTO> GetAllBooks();
        BookDTO GetBookById(int bookId);
        void AddBook(BookDTO book);
        void UpdateBook(BookDTO book);
        void DeleteBook(int bookId);
        List<BookDTO> SearchBooksByName(string searchTerm);
    }
}