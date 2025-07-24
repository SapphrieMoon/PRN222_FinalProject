using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _repo;


        public BookService(IBookRepo repo)
        {
            _repo = repo;
        }

        public List<BookDTO> GetAllBooks()
        {
            var books = _repo.GetAll();
            return books.Select(b => new BookDTO
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Quantity = b.Quantity,
                Avaliable = b.Available,
                ImagePath = b.ImagePath
            }).ToList();
        }

        public BookDTO GetBookById(int id)
        {
            var book = _repo.GetById(id);
            if (book == null) return null;
            return new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Quantity = book.Quantity,
                Avaliable = book.Available,
                ImagePath = book.ImagePath
            };
        }

        public void AddBook(BookDTO bookDto)
        {
            var book = new DAL.Entities.Book
            {
                BookId = bookDto.BookId,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Quantity = bookDto.Quantity,
                Available = bookDto.Avaliable,
                ImagePath = bookDto.ImagePath
            };
            _repo.Add(book);
        }

        public void UpdateBook(BookDTO bookDto)
        {
            var book = new DAL.Entities.Book
            {
                BookId = bookDto.BookId,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Quantity = bookDto.Quantity,
                Available = bookDto.Avaliable,
                ImagePath = bookDto.ImagePath
            };
            _repo.Update(book);
        }

        public void DeleteBook(int bookId)
        {
            _repo.Delete(bookId);
        }

        public List<BookDTO> SearchBooksByName(string searchTerm)
        {
            var books = _repo.SearchByName(searchTerm);
            return books.Select(b => new BookDTO
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Quantity = b.Quantity,
                Avaliable = b.Available,
                ImagePath = b.ImagePath
            }).ToList();
        }
    }
}