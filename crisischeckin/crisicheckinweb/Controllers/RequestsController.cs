using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Models;

namespace crisicheckinweb.Controllers
{
    public class RequestsController : Controller
    {
        private CrisisCheckin db = new CrisisCheckin();

        // GET: Requests
        public async Task<ActionResult> Index()
        {
            var requests = db.Requests.Include(r => r.Creator).Include(r => r.Organization);
            return View(await requests.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.CreatorId = new SelectList(db.Persons, "Id", "FirstName");
            ViewBag.OrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName");
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RequestId,CreatedDate,EndDate,Description,OrganizationId,CreatorId,Completed,Location")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Requests.Add(request);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CreatorId = new SelectList(db.Persons, "Id", "FirstName", request.CreatorId);
            ViewBag.OrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName", request.OrganizationId);
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
            ViewBag.OrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName", request.OrganizationId);
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
            ViewBag.OrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName", request.OrganizationId);
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
