using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public List<BorrowRequest> GetAll(int? requestId, string? accountUserName)
        {
            var query = _context.BorrowRequests
                .Include(r => r.Account)
                .Include(r => r.ProcessedBy)
                .OrderByDescending(i => i.RequestId)
                .AsQueryable();

            if (requestId.HasValue)
            {
                query = query.Where(r => r.RequestId == requestId.Value);
            }

            if (!string.IsNullOrEmpty(accountUserName))
            {
                query = query.Where(r => r.Account.Username.Contains(accountUserName));
            }

            return query.ToList();
        }

        public BorrowRequest GetById(int requestId)
        {
            return _context.BorrowRequests
                .AsNoTracking() //  thêm dòng này để không bị tracked
                .FirstOrDefault(x => x.RequestId == requestId);
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
