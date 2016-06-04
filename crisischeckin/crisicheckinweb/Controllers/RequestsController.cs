using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
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
        public async Task<ActionResult> Index()
        {
            var requests = db.Requests.Include(r => r.Creator).Include(r => r.Assigniees);
            return View(new RequestIndexPageViewModel()
            {
                Requests = await requests.ToListAsync(),
                RequestSearch = new RequestSearch()
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
                                               .Include(r => r.Assigniees)
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
        public async Task<ActionResult> Filter(RequestSearch specifiedRequest)
        {
            var requests = db.Requests.Include(r => r.Creator).Include(r => r.Assigniees);

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
                        requests = requests.Where(x => x.Completed == false && !x.Assigniees.Any());
                        break;
                    case RequestStatus.Assigned:
                        requests = requests.Where(x => x.Completed == false && x.Assigniees.Any());
                        break;
                    case RequestStatus.Completed:
                        requests = requests.Where(x => x.Completed == true);
                        break;
                    default:
                        ModelState.AddModelError("RequestStatus", "Please select a valid request status");
                        break;
                }
            }

            return View("Index", new RequestIndexPageViewModel()
            {
                Requests = await requests.ToListAsync(),
                RequestSearch = new RequestSearch()
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
