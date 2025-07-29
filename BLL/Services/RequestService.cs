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
    public class RequestService : IRequestService
    {
        private readonly IRequestRepo _repo;

        public RequestService(IRequestRepo repo)
        {
            _repo = repo;
        }

        public List<RequestDTO> GetAllRequests(int? requestId, string? accountUserName)
        {
            var requests = _repo.GetAll(requestId, accountUserName);
            return requests.Select(r => new RequestDTO
            {
                RequestId = r.RequestId,
                BookId = r.BookId,
                AccountId = r.AccountId,
                RequestDate = r.RequestDate,
                BorrowDate = r.BorrowDate,
                ReturnDate = r.ReturnDate,
                Status = r.Status,
                ProcessedById = r.ProcessedById,
                ProcessedUserName = r.ProcessedBy?.Username,
                AccountUserName = r.Account?.Username,
                BookTitle = r.Book?.Title
            }).ToList();
        }

        public RequestDTO GetRequestById(int id)
        {
            var request = _repo.GetById(id);
            if (request == null) return null;
            return new RequestDTO
            {
                RequestId = request.RequestId,
                BookId = request.BookId,
                AccountId = request.AccountId,
                RequestDate = request.RequestDate,
                BorrowDate = request.BorrowDate,
                ReturnDate = request.ReturnDate,
                Status = request.Status,
                ProcessedById = request.ProcessedById
            };
        }
        
        public void AddRequest(RequestDTO requestDto)
        {
            var request = new DAL.Entities.BorrowRequest
            {
                BookId = requestDto.BookId,
                AccountId = requestDto.AccountId,
                RequestDate = requestDto.RequestDate,
                BorrowDate = requestDto.BorrowDate,
                ReturnDate = requestDto.ReturnDate,
                Status = requestDto.Status,
                ProcessedById = requestDto.ProcessedById
            };
            _repo.Add(request);
        }

        public void UpdateRequest(RequestDTO requestDto)
        {
            var request = new DAL.Entities.BorrowRequest
            {
                RequestId = requestDto.RequestId,
                BookId = requestDto.BookId,
                AccountId = requestDto.AccountId,
                RequestDate = requestDto.RequestDate,
                BorrowDate = requestDto.BorrowDate,
                ReturnDate = requestDto.ReturnDate,
                Status = requestDto.Status,
                ProcessedById = requestDto.ProcessedById
            };
            _repo.Update(request);
        }

        public void DeleteRequest(int requestId)
        {
            _repo.Delete(requestId);
        }
    }
}
