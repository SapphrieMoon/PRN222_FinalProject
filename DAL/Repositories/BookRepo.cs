using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BookRepo : IBookRepo
    {
        private readonly LibraryDbContext _context;
        public BookRepo(LibraryDbContext context)
        {
            _context = context;
        }
        public List<Book> GetAll()
        {
            return _context.Books.ToList(); // Assuming you want to return all books as a list
        }
        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
        public void Delete(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
