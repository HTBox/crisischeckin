using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IRequest
    {
        IOrderedEnumerable<Request> SortRequests(string sortField, string sortOrder, IEnumerable<Request> requests);
        Task<IEnumerable<Request>> FilterRequestsAsync(RequestSearch specifiedRequest, IQueryable<Request> requests);
        Task<IEnumerable<Request>> GetOpenRequestsAsync();
        Task<IEnumerable<Request>> GetRequestForUserAsync(int userId);
        Task AssignRequestToUserAsync(int userId, int requestId);
        Task CompleteRequestAsync(int requestId);
        RequestSearch CreateRequestSearchObject(DateTime? endDate, 
                                                DateTime? createdDate, 
                                                string location,
                                                string description,
                                                RequestStatus? requestStatus);
    }
}
