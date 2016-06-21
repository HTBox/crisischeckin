using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using crisicheckinweb.Infrastructure.Attributes;
using crisicheckinweb.ViewModels;
using crisicheckinweb.ViewModels.SearchModels;
using crisicheckinweb.Wrappers;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class ResourcesController : Controller
    {
        private CrisisCheckin db = new CrisisCheckin();
        private readonly IWebSecurityWrapper _webSecurity;
        private readonly IResource _resourceSvc;
        private readonly IDisaster _disasterSvc;
        private readonly IOrganizationService _organizationSvc;
        public ResourcesController(IWebSecurityWrapper webSecurity, IResource resourceSvc, IDisaster disasterSvc, IOrganizationService organizationSvc)
        {
            _webSecurity = webSecurity;
            _resourceSvc = resourceSvc;
            _disasterSvc = disasterSvc;
            _organizationSvc = organizationSvc;
        }

        // GET: Resources
        public async Task<ActionResult> Index(int? disasterId = null, int? organizationId = null)
        {
            IEnumerable<Resource> resources = await _resourceSvc.GetAllResourcesAsync();
            if (disasterId.HasValue)
                resources = resources.Where(r => r.DisasterId == disasterId);
            if (organizationId.HasValue)
                resources = resources.Where(r => r.Allocator.OrganizationId == organizationId);
            resources = resources.ToList();
            IEnumerable<ResourceCrudViewModel> models = resources.Select(resource => MapFromResource(resource));

            return View(new AdminResourceIndexViewModel()
            {
                Resources = models,
                ResourceSearch = new ResourceSearch(await db.Disasters.ToListAsync(), await db.Organizations.ToListAsync(), await db.ResourceTypes.ToListAsync()
                    , fixedDisaster: disasterId.HasValue ? _disasterSvc.Get(disasterId.Value) : null
                    , fixedOrganization: organizationId.HasValue ? _organizationSvc.Get(organizationId.Value) : null
                )
            });
        }

        // GET: Resources/Details/5
        public async Task<ActionResult> Details(int? id, string returnUrl = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Resource resource = await _resourceSvc.FindResourceByIdAsync(id);

            if (resource == null)
            {
                return HttpNotFound();
            }

            var model = MapFromResource(resource);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: Resources/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? disasterId = null, int? organizationId = null, string returnUrl = null)
        {
            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name");
            ViewBag.SelectedOrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName");
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName");
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName");

            var model = new ResourceCrudViewModel()
            {
                FixedDisasterId = disasterId,
                FixedOrganizationId = organizationId
            };
            PopulateFixedObjects(model);

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: Resources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ResourceCrudViewModel model, string returnUrl = null)
        {
            // Enforce fixed fields
            if (model.FixedDisasterId.HasValue)
            {
                if (model.DisasterId != model.FixedDisasterId.Value)
                    throw new ArgumentException("Specified disaster id does not match fixed disaster id.");
                model.DisasterId = model.FixedDisasterId.Value;
            }
            if (model.FixedOrganizationId.HasValue)
            {
                if (model.SelectedOrganizationId.HasValue && (model.SelectedOrganizationId.Value != model.FixedOrganizationId.Value))
                    throw new ArgumentException("Specified organization id does not match fixed organization id.");
                model.SelectedOrganizationId = model.FixedOrganizationId;
            }

            // Validate
            if (model.Status == ResourceStatus.All)
                ModelState.AddModelError("Status", "You must select a status other than 'All'.");

            if (model.StartOfAvailability > model.EndOfAvailability)
                ModelState.AddModelError("StartOfAvailability", "The start of the availability for this resource cannot be after the end of its availability.");

            if (ModelState.IsValid)
            {
                Resource resource = MapToResource(model);
                await _resourceSvc.SaveNewResourceAsync(_webSecurity.CurrentUserId, resource);
                return RedirectOrReturn(returnUrl);
            }

            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name", model.DisasterId);
            ViewBag.SelectedOrganizationId = new SelectList(db.Organizations, "OrganizationId", "OrganizationName", model.SelectedOrganizationId);
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName", model.PersonId);
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName", model.ResourceTypeId);
            PopulateFixedObjects(model);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: Resources/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id, string returnUrl = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Resource resource = await _resourceSvc.FindResourceByIdAsync(id);

            if (resource == null)
            {
                return HttpNotFound();
            }

            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name", resource.DisasterId);
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName", resource.PersonId);
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName", resource.ResourceTypeId);

            var model = MapFromResource(resource);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: Resources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Exclude = "Person")] ResourceCrudViewModel model, string returnUrl = null)
        {
            if (model.Status == ResourceStatus.All)
                ModelState.AddModelError("Status", "You must select a status other than 'All'.");

            if (model.StartOfAvailability > model.EndOfAvailability)
                ModelState.AddModelError("StartOfAvailability", "The start of the availability for this resource cannot be after the end of its availability.");

            if (ModelState.IsValid)
            {
                Resource resource = await _resourceSvc.FindResourceByIdAsync(model.ResourceId);
                resource = MapToResource(model, modifying: resource);
                resource = await _resourceSvc.UpdateResourceAsync(resource);
                return RedirectOrReturn(returnUrl);
            }

            ViewBag.DisasterId = new SelectList(db.Disasters, "Id", "Name", model.DisasterId);
            ViewBag.PersonId = new SelectList(db.Persons, "Id", "FirstName", model.PersonId);
            ViewBag.ResourceTypeId = new SelectList(db.ResourceTypes, "ResourceTypeId", "TypeName", model.ResourceTypeId);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: Resources/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id, string returnUrl = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Resource resource = await _resourceSvc.FindResourceByIdAsync(id);

            if (resource == null)
            {
                return HttpNotFound();
            }

            var model = MapFromResource(resource);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id, string returnUrl = null)
        {
            await _resourceSvc.RemoveResourceById(id);
            return RedirectOrReturn(returnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // This method should be refactored into the Resource Service, but 
        // the difficult part would be moving the ResourceSearch class to a place where 
        // the ResourceService can access it.
        [HttpPost, ActionName("Filter")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Filter(ResourceSearch specifiedResource)
        {
            IQueryable<Resource> resources = db.Resources.Include(r => r.Disaster)
                                    .Include(r => r.Allocator)
                                    .Include(r => r.Person)
                                    .Include(r => r.ResourceType);

            if (specifiedResource.Description != null)
            {
                resources = resources.Where(x => x.Description.Contains(specifiedResource.Description));
            }

            if (specifiedResource.NullableCreatedDate != null)
            {
                resources = resources.Where(x => DbFunctions.TruncateTime(x.EntryMade) == DbFunctions.TruncateTime(specifiedResource.NullableCreatedDate));
            }

            if (specifiedResource.NullableStartDate != null)
            {
                resources = resources.Where(x => DbFunctions.TruncateTime(x.StartOfAvailability) == DbFunctions.TruncateTime(specifiedResource.NullableStartDate));
            }

            if (specifiedResource.NullableEndDate != null)
            {
                resources = resources.Where(x => DbFunctions.TruncateTime(x.EndOfAvailability) == DbFunctions.TruncateTime(specifiedResource.NullableEndDate));
            }

            if (specifiedResource.Location.AddressLine1 != null)
            {
                resources = resources.Where(x => x.Location.AddressLine1.Contains(specifiedResource.Location.AddressLine1));
            }

            if (specifiedResource.Location.AddressLine2 != null)
            {
                resources = resources.Where(x => x.Location.AddressLine2.Contains(specifiedResource.Location.AddressLine2));
            }

            if (specifiedResource.Location.AddressLine3 != null)
            {
                resources = resources.Where(x => x.Location.AddressLine3.Contains(specifiedResource.Location.AddressLine3));
            }

            if (specifiedResource.Location.City != null)
            {
                resources = resources.Where(x => x.Location.City.Contains(specifiedResource.Location.City));
            }

            if (specifiedResource.Location.County != null)
            {
                resources = resources.Where(x => x.Location.County.Contains(specifiedResource.Location.County));
            }

            if (specifiedResource.Location.State != null)
            {
                resources = resources.Where(x => x.Location.State.Contains(specifiedResource.Location.State));
            }

            if (specifiedResource.Location.Country != null)
            {
                resources = resources.Where(x => x.Location.Country.Contains(specifiedResource.Location.Country));
            }

            if (specifiedResource.Location.PostalCode != null)
            {
                resources = resources.Where(x => x.Location.PostalCode.Contains(specifiedResource.Location.PostalCode));
            }

            if (specifiedResource.Qty > 0.1m)
            {
                resources = resources.Where(x => x.Qty == specifiedResource.Qty);
            }

            if (specifiedResource.SelectedResourceTypeId != ResourceSearch.GeneralSelectId)
            {
                resources = resources.Where(x => x.ResourceTypeId == specifiedResource.SelectedResourceTypeId);
            }

            if (specifiedResource.FixedDisasterId.HasValue)
            {
                resources = resources.Where(x => x.DisasterId == specifiedResource.FixedDisasterId);
            }
            else if (specifiedResource.SelectedDisasterId != ResourceSearch.GeneralSelectId)
            {
                resources = resources.Where(x => x.DisasterId == specifiedResource.SelectedDisasterId);
            }

            if (specifiedResource.FixedOrganizationId.HasValue)
            {
                resources = resources.Where(x => x.Allocator.OrganizationId == specifiedResource.FixedOrganizationId);
            }
            else if (specifiedResource.SelectedOrganizationId != ResourceSearch.GeneralSelectId)
            {
                resources = resources.Where(x => x.Allocator != null && x.Allocator.OrganizationId == specifiedResource.SelectedOrganizationId);
            }

            if (specifiedResource.Status != ResourceStatus.All)
            {
                resources = resources.Where(x => x.Status == specifiedResource.Status);
            }

            return View("Index", new AdminResourceIndexViewModel()
            {
                Resources = resources.Select(resource => MapFromResource(resource)),
                ResourceSearch = new ResourceSearch(await db.Disasters.ToListAsync(), await db.Organizations.ToListAsync(), await db.ResourceTypes.ToListAsync()
                    , fixedDisaster: specifiedResource.FixedDisasterId.HasValue ? _disasterSvc.Get(specifiedResource.FixedDisasterId.Value) : null
                    , fixedOrganization: specifiedResource.FixedOrganizationId.HasValue ? _organizationSvc.Get(specifiedResource.FixedOrganizationId.Value) : null
                )
            });
        }

        ActionResult RedirectOrReturn(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index");
        }

        void PopulateFixedObjects(ResourceCrudViewModel model)
        {
            if (model.FixedDisasterId.HasValue)
                model.FixedDisaster = _disasterSvc.Get(model.FixedDisasterId.Value);
            if (model.FixedOrganizationId.HasValue)
                model.FixedOrganization = _organizationSvc.Get(model.FixedOrganizationId.Value);
        }

        Resource MapToResource(ResourceCrudViewModel model, Resource modifying = null)
        {
            Resource resource = modifying ?? new Resource();

            // Set on create or modify
            resource.PersonId = model.PersonId;
            resource.Description = model.Description;
            resource.StartOfAvailability = model.StartOfAvailability;
            resource.EndOfAvailability = model.EndOfAvailability;
            resource.Location = model.Location;
            resource.Qty = model.Qty;
            resource.Status = model.Status;
            resource.DisasterId = model.DisasterId;
            resource.ResourceTypeId = model.ResourceTypeId;

            // Only set certain fields when creating, not modifying
            if (modifying == null)
            {
                resource.Allocator = model.SelectedOrganizationId.HasValue ? _organizationSvc.Get(model.SelectedOrganizationId.Value) : null; // Don't allow organization change
                resource.EntryMade = model.EntryMade;
            }

            return resource;
        }

        ResourceCrudViewModel MapFromResource(Resource resource)
        {
            ResourceCrudViewModel model = new ResourceCrudViewModel();

            model.ResourceId = resource.ResourceId;
            model.PersonId = resource.PersonId;
            model.Description = resource.Description;
            model.StartOfAvailability = resource.StartOfAvailability;
            model.EndOfAvailability = resource.EndOfAvailability;
            model.Location = resource.Location;
            model.Qty = resource.Qty;
            model.Status = resource.Status;
            model.DisasterId = resource.DisasterId;
            model.ResourceTypeId = resource.ResourceTypeId;
            model.SelectedOrganizationId = resource.Allocator != null ? resource.Allocator.OrganizationId : (int?)null;
            model.EntryMade = resource.EntryMade;

            model.Disaster = resource.Disaster;
            model.Person = resource.Person;
            model.Allocator = resource.Allocator;
            model.ResourceType = resource.ResourceType;

            return model;
        }
    }
}
