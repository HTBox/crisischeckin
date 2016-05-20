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
        public OrganizationController(IVolunteerService volunteerService, IOrganizationService organizationService,
            IWebSecurityWrapper webSecurityWrapper, IMessageService messageService) : base()
        {
            this.VolunteerService = volunteerService;
            this.OrganizationService = organizationService;
            this.WebSecurityWrapper = webSecurityWrapper;
            this.MessageService = messageService;
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
            Organization org = OrganizationService.Get(id);
            var model = new OrganizationHomeViewModel() { Organization = org };
            return View(model);
        }
    }
}