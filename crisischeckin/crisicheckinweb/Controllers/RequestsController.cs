using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;
using Microsoft.Ajax.Utilities;
using Models;

namespace crisicheckinweb.Controllers
{
    public class RequestsController : Controller
    {
        private CrisisCheckin db = new CrisisCheckin();
        private readonly IWebSecurityWrapper _webSecurity;

        public RequestsController(IWebSecurityWrapper webSecurity)
        {
            _webSecurity = webSecurity;
        }

        // GET: Requests
        public async Task<ActionResult> Index(string sortField, string sortOrder, DateTime? endDate, DateTime? createdDate, string location, string description, RequestStatus? requestStatus)
        {
            var specifiedRequest = CreateRequestSearchObject(endDate, createdDate, location, description, requestStatus);
            if (string.IsNullOrEmpty(sortOrder))
            {
                // If it's blank, default to descending
                sortOrder = "desc";
            }
            else
            {
                // switch sortOrder otherwise
                sortOrder = sortOrder == "desc" ? "asc" : "desc";
            }

            ViewBag.SortOrderParam   = sortOrder;
            ViewBag.SortFieldParam   = String.IsNullOrEmpty(sortOrder) ? "EndDate" : sortField;
            ViewBag.SpecifiedRequest = specifiedRequest;

            IQueryable<Request> requests = db.Requests.Include(r => r.Creator).Include(r => r.Assignee);

            IQueryable<Request> filteredRequests = FilterRequests(specifiedRequest, requests);

            var unsortedRequests = await filteredRequests.ToListAsync();

            IOrderedEnumerable<Request> sortedRequests = SortRequests(sortField, sortOrder, unsortedRequests);

            return View(new RequestIndexPageViewModel()
            {
                Requests = sortedRequests.ToList(),
                RequestSearch = specifiedRequest
            });
        }

        // GET: Requests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.Where(r => r.RequestId == id)
                                               .Include(r => r.Assignee)
                                               .FirstOrDefaultAsync();
            if (request == null)
            {
                return HttpNotFound();
            }

            return View(new RequestDetailsViewModel()
            {
                Request = request
            });
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.CreatorId = _webSecurity.CurrentUserId;
            ViewBag.OrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName");
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EndDate,Description,Location")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedDate = DateTime.Now;
                request.CreatorId = _webSecurity.CurrentUserId;
                request.Completed = false;

                db.Requests.Add(request);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatorId = new SelectList(db.Persons, "Id", "FirstName", request.CreatorId);

            return View(request);
        }

        // POST: Requests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestId,CreatedDate,EndDate,Description,OrganizationId,CreatorId,Completed,Location")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CreatorId = new SelectList(db.Persons, "Id", "FirstName", request.CreatorId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Request request = await db.Requests.FindAsync(id);
            db.Requests.Remove(request);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Filter")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Filter(RequestSearch specifiedRequest, string sortField, string sortOrder)
        {
            IQueryable<Request> requests = db.Requests.Include(r => r.Creator).Include(r => r.Assignee);

            IQueryable<Request> filteredRequests = FilterRequests(specifiedRequest, requests);

            var unsortedRequests = await filteredRequests.ToListAsync();

            IOrderedEnumerable<Request> sortedRequests = SortRequests(sortField, sortOrder, unsortedRequests);

            ViewBag.SortOrderParam   = String.IsNullOrEmpty(sortOrder) ? "desc" : sortOrder;
            ViewBag.SortFieldParam   = String.IsNullOrEmpty(sortOrder) ? "EndDate" : sortField;
            ViewBag.SpecifiedRequest = specifiedRequest;

            return View("Index", new RequestIndexPageViewModel()
            {
                Requests = sortedRequests,
                RequestSearch = specifiedRequest
            });
        }

        [HttpGet, ActionName("VolunteerRequestIndex")]
        public async Task<ActionResult> VolunteerRequestIndex(RequestSearch specifiedRequest)
        {
            var volunteersRequest = await db.Requests
                                            .Include(r => r.Assignee)
                                            .Where(r => r.AssigneeId == _webSecurity.CurrentUserId && r.Completed == false)
                                            .ToListAsync();

            var openRequests = await db.Requests
                                        .Include(r => r.Assignee)
                                        .Where(r => r.Completed == false && !r.AssigneeId.HasValue)
                                        .ToListAsync();

            return View("VolunteerRequestAssignment", new VolunteerRequestIndexViewModel()
            {
                RequestAssignedToVolunteer = volunteersRequest,
                OpenRequests = openRequests
            });
        }

        [HttpPost, ActionName("AssignRequest")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRequest(int requestId)
        {
            var request = db.Requests.FirstOrDefault(r => r.RequestId == requestId);
            if (request != null)
            {
                request.AssigneeId = _webSecurity.CurrentUserId;
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            var volunteersRequest = await db.Requests
                                            .Include(r => r.Assignee)
                                            .Where(r => r.AssigneeId == _webSecurity.CurrentUserId && r.Completed == false)
                                            .ToListAsync();

            var openRequests = await db.Requests
                                        .Include(r => r.Assignee)
                                        .Where(r => r.Completed == false && !r.AssigneeId.HasValue)
                                        .ToListAsync();

            return View("VolunteerRequestAssignment", new VolunteerRequestIndexViewModel()
            {
                RequestAssignedToVolunteer = volunteersRequest,
                OpenRequests = openRequests
            });
        }

        [HttpPost, ActionName("CompleteRequest")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteRequest(int requestId)
        {
            var request = db.Requests.FirstOrDefault(r => r.RequestId == requestId);
            if (request != null)
            {
                request.Completed = true;
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            var volunteersRequest = await db.Requests
                                            .Include(r => r.Assignee)
                                            .Where(r => r.AssigneeId == _webSecurity.CurrentUserId && r.Completed == false)
                                            .ToListAsync();

            var openRequests = await db.Requests
                                        .Include(r => r.Assignee)
                                        .Where(r => r.Completed == false && !r.AssigneeId.HasValue)
                                        .ToListAsync();

            return View("VolunteerRequestAssignment", new VolunteerRequestIndexViewModel()
            {
                RequestAssignedToVolunteer = volunteersRequest,
                OpenRequests = openRequests
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static IOrderedEnumerable<Request> SortRequests(string sortField, string sortOrder, List<Request> requests)
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

        private IQueryable<Request> FilterRequests(RequestSearch specifiedRequest, IQueryable<Request> requests)
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
                        ModelState.AddModelError("RequestStatus", "Please select a valid request status");
                        break;
                }
            }
            return requests;
        }

        private RequestSearch CreateRequestSearchObject(DateTime? endDate, DateTime? createdDate, string location, string description,
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
