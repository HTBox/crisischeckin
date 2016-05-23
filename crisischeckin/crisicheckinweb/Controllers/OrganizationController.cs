﻿using System;
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
            int personId = 0;

            if (person != null) personId = person.Id; 

            var model = new NewOrganizationViewModel()
            {
                Address = new Address(),
                OrganizationName = "",
                Type = OrganizationTypeEnum.Local,
                UserIdRegisteringOrganization = personId
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


            // Now pass the new organization out through the Organisation service so it can be persisted in the database.
            newOrg = OrganizationService.AddOrganization(newOrg);

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
                Person person = VolunteerService.FindByUserId(newOrganization.UserIdRegisteringOrganization);
                MessageService.SendMessage(message, person, "CrisisCheckin");

            });


            // Send the verification email out to the user.
            return View();
        }

        public ActionResult VerifyOrganization(int id)
        {
            OrganizationService.VerifyOrganization(id);

            return View("OrganizationVerified");
        }

        public ActionResult Home(int id)
        {
            return View(CreateHomeViewModel(id));
        }

        [HttpPost]
        public ActionResult CheckinResource(OrganizationHomeViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Home", CreateHomeViewModel(model));

            try
            {
                var person = VolunteerService.FindByUserId(WebSecurityWrapper.CurrentUserId);
                if ((person == null) || (person.OrganizationId != model.OrganizationId))
                {
                    throw new ArgumentException(
                        "The logged in user is either the administrator or is not a member of the specified organisation."); // Should be verifying access to entire page?
                }
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

        OrganizationHomeViewModel CreateHomeViewModel(int organizationId)
        {
            return CreateHomeViewModel(new OrganizationHomeViewModel
            {
                OrganizationId = organizationId
            });
        }

        OrganizationHomeViewModel CreateHomeViewModel(OrganizationHomeViewModel inputModel)
        {
            int id = inputModel.OrganizationId;
            Organization org = OrganizationService.Get(id);

            var resources = AdminService.GetResourceCheckinsForOrganization(id);
            var resourceTypes = AdminService.GetResourceTypes();
            var allDisasters = DisasterService.GetActiveList();

            var model = new OrganizationHomeViewModel
            {
                OrganizationId = id,
                Organization = org,
                OrganizationResources = resources,
                ResourceTypes = resourceTypes,
                AllDisasters = allDisasters
            };

            model.ResourceDisasterId = inputModel.ResourceDisasterId;
            model.ResourceDescription = inputModel.ResourceDescription;
            model.ResourceQuantity = inputModel.ResourceQuantity;
            model.ResourceTypeId = inputModel.ResourceTypeId;
            model.ResourceStartDate = inputModel.ResourceStartDate;
            model.ResourceEndDate = inputModel.ResourceEndDate;
            model.ResourceLocation = inputModel.ResourceLocation;

            return model;
        }
    }
}