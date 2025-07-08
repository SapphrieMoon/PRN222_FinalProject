using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRequestRepo
    {
        List<BorrowRequest> GetAll();
        BorrowRequest GetById(int RequestId);
        void Add(BorrowRequest request);
        void Update(BorrowRequest request);
        void Delete(int requestId);
    }
}
