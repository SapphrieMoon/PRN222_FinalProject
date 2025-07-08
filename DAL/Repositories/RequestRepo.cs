using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RequestRepo : IRequestRepo
    {
        private readonly LibraryDbContext _context;

        public RequestRepo(LibraryDbContext context)
        {
            _context = context;
        }

        public List<BorrowRequest> GetAll()
        {
            return _context.BorrowRequests.ToList(); // Assuming you want to return all requests as a list
        }

        public BorrowRequest GetById(int requestId)
        {
            return _context.BorrowRequests.Find(requestId); // Find the request by its ID
        }

        public void Add(BorrowRequest request)
        {
            _context.BorrowRequests.Add(request);
            _context.SaveChanges(); // Save changes to the database
        }

        public void Update(BorrowRequest request)
        {
            _context.BorrowRequests.Update(request);
            _context.SaveChanges(); // Save changes to the database
        }

        public void Delete(int requestId)
        {
            var request = _context.BorrowRequests.Find(requestId);
            if (request != null)
            {
                _context.BorrowRequests.Remove(request);
                _context.SaveChanges(); // Save changes to the database
            }
        }
    }
}
