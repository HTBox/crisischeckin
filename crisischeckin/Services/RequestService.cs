using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Interfaces;

namespace Services
{
    public class RequestService : IRequest
    {
        private readonly IDataService _dataService;

        public RequestService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            _dataService = service;
        }


        public IOrderedEnumerable<Request> SortRequests(string sortField, string sortOrder, IEnumerable<Request> requests)
        {
            IOrderedEnumerable<Request> orderRequest = null;

            if (requests != null)
            {
                switch (sortField)
                {
                    case "End Date":
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.EndDate)
                            : requests.OrderBy(r => r.EndDate);
                        break;
                    case "Location":
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.Location)
                            : requests.OrderBy(r => r.Location);
                        break;
                    case "Created By":
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.Creator.FullName)
                            : requests.OrderBy(r => r.Creator.FullName);
                        break;
                    case "Created On":
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.CreatedDate)
                            : requests.OrderBy(r => r.CreatedDate);
                        break;
                    case "Status":
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.Completed)
                            : requests.OrderBy(r => r.Completed);
                        break;
                    case "Description":
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.Description)
                            : requests.OrderBy(r => r.Description);
                        break;
                    default:
                        orderRequest = sortOrder == "desc"
                            ? requests.OrderByDescending(r => r.EndDate)
                            : requests.OrderBy(r => r.EndDate);
                        break;
                }
            }
            return orderRequest;
        }

        public async Task<IEnumerable<Request>> FilterRequestsAsync(RequestSearch specifiedRequest, IQueryable<Request> requests)
        {
            if (specifiedRequest.Description != null)
            {
                requests = requests.Where(x => x.Description.Contains(specifiedRequest.Description));
            }

            if (specifiedRequest.Location != null)
            {
                requests = requests.Where(x => x.Location.Contains(specifiedRequest.Location));
            }

            if (specifiedRequest.NullableCreatedDate != null)
            {
                requests = requests.Where(x => x.CreatedDate == specifiedRequest.NullableCreatedDate);
            }

            if (specifiedRequest.NullableEndDate != null)
            {
                requests = requests.Where(x => x.EndDate == specifiedRequest.NullableEndDate);
            }

            if (specifiedRequest.RequestStatus != RequestStatus.All)
            {
                switch (specifiedRequest.RequestStatus)
                {
                    case RequestStatus.Unassigned:
                        requests = requests.Where(x => x.Completed == false && !x.AssigneeId.HasValue);
                        break;
                    case RequestStatus.Assigned:
                        requests = requests.Where(x => x.Completed == false && x.AssigneeId.HasValue);
                        break;
                    case RequestStatus.Completed:
                        requests = requests.Where(x => x.Completed == true);
                        break;
                    default:
                        throw new ArgumentException("A valid request status was not selected.");
                }
            }
            return await requests.ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetOpenRequestsAsync()
        {
            return await _dataService.Requests
                            .Include(r => r.Assignee)
                            .Where(r => r.Completed == false && !r.AssigneeId.HasValue)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetRequestForUserAsync(int userId)
        {
            return await _dataService.Requests.Include(r => r.Assignee)
                                    .Where(r => r.AssigneeId == userId && r.Completed == false)
                                    .ToListAsync();
        }

        public async Task AssignRequestToUserAsync(int userId, int requestId)
        {
            await _dataService.AssignRequestToUserAsync(userId, requestId);
        }

        public async Task CompleteRequestAsync(int requestId)
        {
            await _dataService.CompleteRequestAsync(requestId);
        }

        public RequestSearch CreateRequestSearchObject(DateTime? endDate, DateTime? createdDate, string location, string description,
            RequestStatus? requestStatus)
        {
            var nonNullRequestStatus = requestStatus ?? RequestStatus.All;
            return new RequestSearch()
            {
                NullableCreatedDate = createdDate,
                NullableEndDate = endDate,
                Location = location,
                Description = description,
                RequestStatus = nonNullRequestStatus
            };
        }
    }
}
