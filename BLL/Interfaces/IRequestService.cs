using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRequestService
    {
        List<RequestDTO> GetAllRequests();
        RequestDTO GetRequestById(int requestId);
        void AddRequest(RequestDTO request);
        void UpdateRequest(RequestDTO request);
        void DeleteRequest(int requestId);

    }
}
