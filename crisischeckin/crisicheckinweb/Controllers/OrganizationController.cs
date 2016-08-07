using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;
using Models;
using Services;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class OrganizationController : Controller
    {
        protected IVolunteerService VolunteerService;
        protected IOrganizationService OrganizationService;
        protected IWebSecurityWrapper WebSecurityWrapper;
        protected IMessageService MessageService;
        protected IAdmin AdminService;
        protected IDisaster DisasterService;
        public OrganizationController(IVolunteerService volunteerService, IOrganizationService organizationService,
            IWebSecurityWrapper webSecurityWrapper, IMessageService messageService, IAdmin adminService, IDisaster disasterService) : base()
        {
            this.VolunteerService = volunteerService;
            this.OrganizationService = organizationService;
            this.WebSecurityWrapper = webSecurityWrapper;
            this.MessageService = messageService;
            this.AdminService = adminService;
            this.DisasterService = disasterService;
        }

        public ActionResult List()
        {
            var organizations = OrganizationService.GetActiveList();
            return View(organizations);
        }

        public ActionResult RegisterNewOrganization()
        {
            var person = VolunteerService.FindByUserId(WebSecurityWrapper.CurrentUserId);
            if (person == null)
                throw new InvalidOperationException("A volunteer must be logged in to register a new organization.");

            var model = new NewOrganizationViewModel()
            {
                Address = new Address(),
                OrganizationName = "",
                Type = OrganizationTypeEnum.Local
            };

            return View(model);

        }

        [HttpPost]
        public ActionResult ProcessNewOrganization(NewOrganizationViewModel newOrganization)
        {
            if (!this.ModelState.IsValid)
            {
                return View("RegisterNewOrganization", newOrganization);
            }

            // If execution gets here, we have valid details. The first thing we will do is to create the organisation.
            // Let's build an organisation...

            Organization newOrg = new Organization();
            newOrg.Verified = false;
            newOrg.OrganizationName = newOrganization.OrganizationName;
            newOrg.Location = newOrganization.Address;
            newOrg.Type = newOrganization.Type;

            // TODO POint of contact?


            // Get registering person to make them the organization admin
            Person person = GetCurrentVolunteer();
            if (person == null)
                throw new ArgumentException("Logged-in person not found or is an administrator.");

            // Now pass the new organization out through the Organisation service so it can be persisted in the database.
            newOrg = OrganizationService.AddOrganization(newOrg, person.Id);

            Task.Run(() =>
            {
                var routeValues = new RouteValueDictionary();
                routeValues.Add("organizationId", newOrg.OrganizationId);
                var organizationVerificationLink = Url.Action("VerifyOrganization", "Organization", routeValues,
                    Request.Url.Scheme);
                var body =
                    string.Format(
                        @"<p>Click on the following link to verify the new organization : <a href='{0}'>{0}</a> </p>",
                        organizationVerificationLink);
                var message = new Message("CrisisCheckin - Verify your organization", body);
                MessageService.SendMessage(message, person, "CrisisCheckin");

            });


            // Send the verification email out to the user.
            return View();
        }

        public ActionResult VerifyOrganization(int organizationId)
        {
            OrganizationService.VerifyOrganization(organizationId);

            return View("OrganizationVerified");
        }

        public ActionResult Home(int? id)
        {
            if (id.HasValue)
            {
                // Check user can see this org
                ConfirmAccess(id.Value);
            }
            else
            {
                // Get current person and show their organization
                Person person = GetCurrentVolunteer();
                if ((person == null) || (!person.OrganizationId.HasValue))
                    throw new InvalidOperationException("The logged-in user is either an administrator or is not associated with an organization.");
                id = person.OrganizationId;
            }

            return View(CreateHomeViewModel(id.Value));
        }

        [HttpPost]
        public ActionResult CheckinResource(OrganizationHomeViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                // Check access
                var person = GetCurrentVolunteer();
                ConfirmAccess(model.OrganizationId);

                // Ok
                model.Organization = OrganizationService.Get(model.OrganizationId);
                DisasterService.AddResourceCheckIn(model.Organization, person, model.ResourceDisasterId, model.ResourceDescription,
                    model.ResourceQuantity, model.ResourceTypeId, model.ResourceStartDate, model.ResourceEndDate, model.ResourceLocation);

                return Redirect("Home/" + model.OrganizationId);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Home", CreateHomeViewModel(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveResource(OrganizationHomeViewModel model)
        {
            ModelState.RemoveErrorsExcept(nameof(OrganizationHomeViewModel.RemoveResourceId));
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                // Check access
                ConfirmAccess(model.OrganizationId);

                // Check the resource ID
                var resources = AdminService.GetResourceCheckinsForOrganization(model.OrganizationId);
                var resource = resources.FirstOrDefault(r => r.ResourceId == model.RemoveResourceId);
                if (resource == null)
                {
                    throw new ArgumentException("The specified resource does not belong to this Organization.");
                }

                // Ok
                DisasterService.RemoveResourceById(resource.ResourceId);

                return Redirect("Home/" + model.OrganizationId);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Home", CreateHomeViewModel(model.OrganizationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVolunteer(OrganizationHomeViewModel model)
        {
            ModelState.RemoveErrorsExcept("OrganizationId,AddVolunteerId");
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                ConfirmAdminAccess(model.OrganizationId);
                VolunteerService.AddVolunteerToOrganization(model.OrganizationId, model.AddVolunteerId);

                return Redirect("Home/" + model.OrganizationId);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Home", CreateHomeViewModel(model.OrganizationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveVolunteer(OrganizationHomeViewModel model)
        {
            ModelState.RemoveErrorsExcept("OrganizationId,RemoveVolunteerId");
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                ConfirmAdminAccess(model.OrganizationId);
                VolunteerService.RemoveVolunteerFromOrganization(model.OrganizationId, model.RemoveVolunteerId);

                return Redirect("Home/" + model.OrganizationId);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Home", CreateHomeViewModel(model.OrganizationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoteToAdmin(OrganizationHomeViewModel model)
        {
            ModelState.RemoveErrorsExcept("OrganizationId,PromoteToAdminPersonId");
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                ConfirmAdminAccess(model.OrganizationId);
                VolunteerService.PromoteVolunteerToOrganizationAdmin(model.OrganizationId, model.PromoteToAdminPersonId);

                return Redirect("Home/" + model.OrganizationId);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Home", CreateHomeViewModel(model.OrganizationId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DemoteFromAdmin(OrganizationHomeViewModel model)
        {
            ModelState.RemoveErrorsExcept("OrganizationId,DemoteFromAdminPersonId");
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                ConfirmAdminAccess(model.OrganizationId);
                VolunteerService.DemoteVolunteerFromOrganizationAdmin(model.OrganizationId, model.DemoteFromAdminPersonId);

                return Redirect("Home/" + model.OrganizationId);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Home", CreateHomeViewModel(model.OrganizationId));
        }

        OrganizationHomeViewModel CreateHomeViewModel(int organizationId)
        {
            return CreateHomeViewModel(new OrganizationHomeViewModel
            {
                OrganizationId = organizationId
            });
        }

        OrganizationHomeViewModel CreateHomeViewModel(OrganizationHomeViewModel inputModel)
        {
            // Organization
            int id = inputModel.OrganizationId;
            Organization org = OrganizationService.Get(id);

            // Access level
            var isAdministrator = WebSecurityWrapper.IsUserInRole(Common.Constants.RoleAdmin);
            var isOrgAdmin = HasOrgAdminAccess(id);

            // Volunteers
            var volunteers = VolunteerService.GetVolunteersByOrganization(id).OrderBy(p => p.IsOrganizationAdmin).Reverse();

            // Resources
            var resources = AdminService.GetResourceCheckinsForOrganization(id);
            var resourceTypes = AdminService.GetResourceTypes();

            // Commitments
            var commitments = VolunteerService.GetCommitmentsForOrganization(id, false);

            // Lookup lists
            var allDisasters = DisasterService.GetActiveList();
            var volunteersSelectList = VolunteerService.GetList().OrderBy(person => person.LastName).Select(person => new SelectListItem()
            {
                Value = person.Id.ToString(),
                Text = $"{person.LastName}, {person.FirstName} - {person.Email}",
                Selected = (person.Id == inputModel.AddVolunteerId)
            });

            // Disaster info breakdown - get all disasters with any volunteer or resource checkin
            var disasters = GetDisasterInfos(allDisasters, resources, commitments);

            // Create
            var model = new OrganizationHomeViewModel
            {
                OrganizationId = id,
                Organization = org,
                IsAdministrator = isAdministrator,
                IsOrgAdmin = isOrgAdmin,
                Volunteers = volunteers,
                OrganizationResources = resources,
                Disasters = disasters,
                ResourceTypes = resourceTypes,
                AllDisasters = allDisasters,
                VolunteersSelectList = volunteersSelectList
            };

            // Resources form fields
            model.ResourceDisasterId = inputModel.ResourceDisasterId;
            model.ResourceDescription = inputModel.ResourceDescription;
            model.ResourceQuantity = inputModel.ResourceQuantity;
            model.ResourceTypeId = inputModel.ResourceTypeId;
            model.ResourceStartDate = inputModel.ResourceStartDate;
            model.ResourceEndDate = inputModel.ResourceEndDate;
            model.ResourceLocation = inputModel.ResourceLocation;
            model.RemoveResourceId = inputModel.RemoveResourceId;

            return model;
        }

        IEnumerable<OrganizationDisasterViewModel> GetDisasterInfos(IEnumerable<Disaster> allDisasters, IEnumerable<Resource> resources, IEnumerable<Commitment> commitments)
        {
            // Commitments grouped by disaster
            var commitmentsByDisaster = (from commitment in commitments
                                         group commitment by commitment.DisasterId into disasterCommitments
                                         select disasterCommitments);

            // Resources grouped by disaster
            var resourcesByDisaster = (from resource in resources.Where(r => r.Disaster.IsActive == true)
                                       group resource by resource.DisasterId into disasterResources
                                       select disasterResources);

            // Join both lists together (effectively a double outer join)
            var allDisasterIdsWithCheckins = from disasterId in
                                    ((from cd in commitmentsByDisaster
                                      select cd.Key)
                                        .Union(
                                            from rd in resourcesByDisaster
                                            select rd.Key)
                                    )
                                             select disasterId;

            // Result
            var disasters = from disasterId in allDisasterIdsWithCheckins
                            join disaster in allDisasters on disasterId equals disaster.Id
                            join cd in commitmentsByDisaster on disasterId equals cd.Key into groupJoinC
                            join rd in resourcesByDisaster on disasterId equals rd.Key into groupJoinR
                            from leftJoinC in groupJoinC.DefaultIfEmpty()
                            from leftJoinR in groupJoinR.DefaultIfEmpty()
                            select new OrganizationDisasterViewModel()
                            {
                                Disaster = new DisasterViewModel()
                                {
                                    Id = disaster.Id,
                                    IsActive = disaster.IsActive,
                                    Name = disaster.Name
                                },
                                Commitments = leftJoinC == null ? new List<Commitment>() : leftJoinC.ToList(),
                                Resources = leftJoinR == null ? new List<Resource>() : leftJoinR.ToList()
                            };
            return disasters;
        }

        Person GetCurrentVolunteer()
        {
            return VolunteerService.FindByUserId(WebSecurityWrapper.CurrentUserId);
        }

        /// <summary>
        /// Returns true if the user is an administrator user or is associated with this organization.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool HasAccess(int organizationId)
        {
            if (WebSecurityWrapper.IsUserInRole(Common.Constants.RoleAdmin))
                return true;

            var person = GetCurrentVolunteer();
            if (person.OrganizationId == organizationId)
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if the user is an administrator or an organization admin.
        /// </summary>
        bool HasAdminAccess(int organizationId)
        {
            if (WebSecurityWrapper.IsUserInRole(Common.Constants.RoleAdmin))
                return true;

            return HasOrgAdminAccess(organizationId);
        }

        /// <summary>
        /// Returns true only if the user is an organization admin.
        /// </summary>
        bool HasOrgAdminAccess(int organizationId)
        {
            Person currentPerson = GetCurrentVolunteer();
            if (currentPerson == null)
                return false;
            if ((currentPerson.OrganizationId == organizationId) && currentPerson.IsOrganizationAdmin)
                return true;

            return false;
        }

        void ConfirmAccess(int organizationId)
        {
            if (!HasAccess(organizationId))
                throw new InvalidOperationException("You do not have access to this organization");
        }

        void ConfirmAdminAccess(int organizationId)
        {
            if (!HasAdminAccess(organizationId))
                throw new InvalidOperationException("You do not have administrative privelages for this organization.");
        }

        void ConfirmOrgAdminAccess(int organizationId)
        {
            if (!HasOrgAdminAccess(organizationId))
                throw new InvalidOperationException("You are not an administrator for this organization.");
        }
    }
}