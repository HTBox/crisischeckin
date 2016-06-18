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
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class RequestsController : Controller
    {
        private CrisisCheckin db = new CrisisCheckin();
        private readonly IWebSecurityWrapper _webSecurity;
        private readonly IRequest _requestSvc;

        public RequestsController(IWebSecurityWrapper webSecurity, IRequest requestSvc)
        {
            _webSecurity = webSecurity;
            _requestSvc = requestSvc;
        }

        // GET: Requests
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string sortField, string sortOrder, DateTime? endDate, DateTime? createdDate, string location, string description, RequestStatus? requestStatus)
        {
            var specifiedRequest = _requestSvc.CreateRequestSearchObject(endDate, createdDate, location, description, requestStatus);
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

            IQueryable<Request> requests = db.Requests.Include(r => r.Creator).Include(r => r.Assignee).AsQueryable();

            IEnumerable<Request> filteredRequests = await _requestSvc.FilterRequestsAsync(specifiedRequest, requests);

            IOrderedEnumerable<Request> sortedRequests = _requestSvc.SortRequests(sortField, sortOrder, filteredRequests);

            return View(new AdminRequestIndexViewModel()
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CreatorId = _webSecurity.CurrentUserId;
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Request request = await db.Requests.FindAsync(id);
            db.Requests.Remove(request);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Filter")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Filter(RequestSearch specifiedRequest, string sortField, string sortOrder)
        {
            IQueryable<Request> requests = db.Requests.Include(r => r.Creator).Include(r => r.Assignee);

            IEnumerable<Request> filteredRequests = await _requestSvc.FilterRequestsAsync(specifiedRequest, requests);

            IOrderedEnumerable<Request> sortedRequests = _requestSvc.SortRequests(sortField, sortOrder, filteredRequests);

            ViewBag.SortOrderParam   = String.IsNullOrEmpty(sortOrder) ? "desc" : sortOrder;
            ViewBag.SortFieldParam   = String.IsNullOrEmpty(sortOrder) ? "EndDate" : sortField;
            ViewBag.SpecifiedRequest = specifiedRequest;

            return View("Index", new AdminRequestIndexViewModel()
            {
                Requests = sortedRequests,
                RequestSearch = specifiedRequest
            });
        }

        [HttpGet, ActionName("VolunteerRequestIndex")]
        [Authorize]
        public async Task<ActionResult> VolunteerRequestIndex(RequestSearch specifiedRequest)
        {
            var volunteersRequest = await _requestSvc.GetRequestForUserAsync(_webSecurity.CurrentUserId);

            var openRequests = await _requestSvc.GetOpenRequestsAsync();

            return View("VolunteerRequestAssignment", new VolunteerRequestIndexViewModel()
            {
                RequestAssignedToVolunteer = volunteersRequest,
                OpenRequests = openRequests
            });
        }

        [HttpPost, ActionName("AssignRequest")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> AssignRequest(int requestId)
        {
            await _requestSvc.AssignRequestToUserAsync(_webSecurity.CurrentUserId, requestId);

            var volunteersRequest = await _requestSvc.GetRequestForUserAsync(_webSecurity.CurrentUserId);

            var openRequests = await _requestSvc.GetOpenRequestsAsync();

            return View("VolunteerRequestAssignment", new VolunteerRequestIndexViewModel()
            {
                RequestAssignedToVolunteer = volunteersRequest,
                OpenRequests = openRequests
            });
        }

        [HttpPost, ActionName("CompleteRequest")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> CompleteRequest(int requestId)
        {
            await _requestSvc.CompleteRequestAsync(requestId);

            var volunteersRequest = await _requestSvc.GetRequestForUserAsync(_webSecurity.CurrentUserId);

            var openRequests = await _requestSvc.GetOpenRequestsAsync();

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
    }
}
