using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using crisicheckinweb.ViewModels.SearchModels;
using crisicheckinweb.Wrappers;
using Models;
using WebMatrix.WebData;

namespace crisicheckinweb.Controllers
{
    public class ResourcesController : Controller
    {
        private CrisisCheckin db = new CrisisCheckin();
        private readonly IWebSecurityWrapper _webSecurity ;

        public ResourcesController(IWebSecurityWrapper webSecurity)
        {
            _webSecurity = webSecurity;
        }

        // GET: Resources
        public async Task<ActionResult> Index(IQueryable<Resource> filteredResources = null)
        {
            IQueryable<Resource> resources;

            resources = filteredResources ??
                        db.Resources.Include(r => r.Disaster)
                                    .Include(r => r.Person)
                                    .Include(r => r.ResourceType);

            return View(new AdminResourceIndexViewModel()
            {
                Resources =  await resources.ToListAsync(),
                ResourceSearch = new ResourceSearch(await db.Disasters.ToListAsync(), await db.ResourceTypes.ToListAsync())
            });
        }

        // GET: Resources/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = await db.Resources.FindAsync(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // GET: Resources/Create
        public ActionResult Create()
        {
            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name");
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName");
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName");
            return View();
        }

        // POST: Resources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ResourceId,Description,StartOfAvailability,EndOfAvailability,Location,Qty,Status,DisasterId,ResourceTypeId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                resource.PersonId = _webSecurity.CurrentUserId;
                resource.EntryMade = DateTime.Now;

                db.Resources.Add(resource);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name", resource.DisasterId);
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName", resource.PersonId);
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName", resource.ResourceTypeId);
            return View(resource);
        }

        // GET: Resources/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = await db.Resources.FindAsync(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name", resource.DisasterId);
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName", resource.PersonId);
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName", resource.ResourceTypeId);
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ResourceId,EntryMade,PersonId,Description,StartOfAvailability,EndOfAvailability,Location,Qty,Status,DisasterId,ResourceTypeId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name", resource.DisasterId);
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName", resource.PersonId);
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName", resource.ResourceTypeId);
            return View(resource);
        }

        // GET: Resources/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = await db.Resources.FindAsync(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Resource resource = await db.Resources.FindAsync(id);
            db.Resources.Remove(resource);
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

        [HttpPost, ActionName("Filter")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Filter(ResourceSearch specifiedResource)
        {
            var resource = db.Resources.Include(r => r.Disaster)
                                    .Include(r => r.Person)
                                    .Include(r => r.ResourceType);

            if (specifiedResource.Description != null)
            {
                resource = resource.Where(x => x.Description.Contains(specifiedResource.Description));
            }

            if (specifiedResource.NullableCreatedDate != null)
            {
                resource = resource.Where(x => DbFunctions.TruncateTime(x.EntryMade) == DbFunctions.TruncateTime(specifiedResource.NullableCreatedDate));
            }

            if (specifiedResource.NullableStartDate != null)
            {
                resource = resource.Where(x => DbFunctions.TruncateTime(x.StartOfAvailability) == DbFunctions.TruncateTime(specifiedResource.NullableStartDate));
            }

            if (specifiedResource.NullableEndDate != null)
            {
                resource = resource.Where(x => DbFunctions.TruncateTime(x.EndOfAvailability) == DbFunctions.TruncateTime(specifiedResource.NullableEndDate));
            }

            if (specifiedResource.Location.AddressLine1 != null)
            {
                resource = resource.Where(x => x.Location.AddressLine1.Contains(specifiedResource.Location.AddressLine1));
            }

            if (specifiedResource.Location.AddressLine2 != null)
            {
                resource = resource.Where(x => x.Location.AddressLine2.Contains(specifiedResource.Location.AddressLine2));
            }

            if (specifiedResource.Location.AddressLine3 != null)
            {
                resource = resource.Where(x => x.Location.AddressLine3.Contains(specifiedResource.Location.AddressLine3));
            }

            if (specifiedResource.Location.City != null)
            {
                resource = resource.Where(x => x.Location.City.Contains(specifiedResource.Location.City));
            }

            if (specifiedResource.Location.County != null)
            {
                resource = resource.Where(x => x.Location.County.Contains(specifiedResource.Location.County));
            }

            if (specifiedResource.Location.State != null)
            {
                resource = resource.Where(x => x.Location.State.Contains(specifiedResource.Location.State));
            }

            if (specifiedResource.Location.Country != null)
            {
                resource = resource.Where(x => x.Location.Country.Contains(specifiedResource.Location.Country));
            }

            if (specifiedResource.Location.PostalCode != null)
            {
                resource = resource.Where(x => x.Location.PostalCode.Contains(specifiedResource.Location.PostalCode));
            }

            if (specifiedResource.Qty > 0.1m)
            {
                resource = resource.Where(x => x.Qty == specifiedResource.Qty);
            }

            if (specifiedResource.SelectedResourceTypeId != null)
            {
                resource = resource.Where(x => x.ResourceTypeId == specifiedResource.SelectedResourceTypeId);
            }

            if (specifiedResource.SelectedDisasterId != null)
            {
                resource = resource.Where(x => x.DisasterId == specifiedResource.SelectedDisasterId);
            }

            if (specifiedResource.Status != null)
            {
                resource = resource.Where(x => x.Status == specifiedResource.Status);
            }

            // Left Off - 
            return RedirectToAction("Index", resource);
        }
    }
}
