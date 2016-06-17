using Services;
using Services.Interfaces;
using System;
using System.Web.Mvc;
using System.Linq;
using crisicheckinweb.ViewModels;
using Common;
using Models;
using System.Collections.Generic;
using crisicheckinweb.Wrappers;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : BaseController
    {
        private readonly IDisaster _disasterSvc;
        private readonly ICluster _clusterSvc;
        private readonly IAdmin _adminSvc;
        private readonly IMessageService _messageSvc;
        private readonly IVolunteerService _volunteerSvc;
        private readonly IWebSecurityWrapper _webSecurity;

        public VolunteerController(IDisaster disasterSvc, ICluster clusterSvc, IAdmin adminSvc, IMessageService messageSvc, IVolunteerService volunteerSvc, IWebSecurityWrapper webSecurity)
        {
            _disasterSvc = disasterSvc;
            _clusterSvc = clusterSvc;
            _adminSvc = adminSvc;
            _messageSvc = messageSvc;
            _volunteerSvc = volunteerSvc;
            _webSecurity = webSecurity;
        }

        [HttpGet]
        public ActionResult ListByDisaster()
        {
            var model = new ListByDisasterViewModel { Disasters = _disasterSvc.GetActiveList(), CommitmentDate = null };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContact(ListByDisasterViewModel model)
        {
            try
            {
                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
                if (!person.OrganizationId.HasValue)
                {
                    throw new ArgumentException("Signed in User is not part of an Organization");
                }

                _adminSvc.AddContactForOrganization(person.OrganizationId.Value, model.AddContactId);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var viewModel = new ListByDisasterViewModel
            {
                Disasters = _disasterSvc.GetActiveList(),
                SelectedDisaster = model.SelectedDisaster,
                CommitmentDate = model.CommitmentDate
            };
            return View("ListByDisaster", viewModel);
        }


        [HttpGet]
        public ActionResult ListResourceCheckinsByDisaster()
        {
            var model = new ListByDisasterViewModel { Disasters = _disasterSvc.GetActiveList(), CommitmentDate = null };
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateMessageToVolunteersByDisaster(int id)
        {
            var model = new SendMessageToAllVolunteersByDisasterViewModel { DisasterId = id };
            PopulateSendMessageViewModel(model);
            // Default subject to disaster name so they can type just a message if they want to.
            model.Subject = model.DisasterName;
            return View("CreateMessage", model);
        }

        private void PopulateSendMessageViewModel(SendMessageToAllVolunteersByDisasterViewModel model)
        {
            model.DisasterName = _disasterSvc.GetName(model.DisasterId);
            model.Clusters = _clusterSvc.GetList();
        }

        [HttpPost]
        public ActionResult SendMessageToVolunteersByDisaster(SendMessageToAllVolunteersByDisasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateSendMessageViewModel(model);
                return View("CreateMessage", model);
            }
            else if (model.IsSMSMessage && model.Message.Length > Constants.TwilioMessageLength)
            {
                PopulateSendMessageViewModel(model);
                ModelState["Message"].Errors.Add(string.Format("The message cannot have more than {0} characters when submiting as a SMS.", Constants.TwilioMessageLength));
                return View("CreateMessage", model);
            }

            var sender = model.DisasterName + " - Coordinator";
            var recipientCriterion = new RecipientCriterion(model.DisasterId, model.SelectedClusterIds, model.ClusterCoordinatorsOnly, model.CheckedInOnly);
            var message = new Message(model.Subject, model.Message) { IsSMSMessage = model.IsSMSMessage };

            _messageSvc.SendMessageToDisasterVolunteers(message, recipientCriterion, sender);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Filter(ListByDisasterViewModel model)
        {
            var result = new CheckinListsResultsViewModel();

            if (model.SelectedDisaster != 0)
            {
                result.ResourceCheckins = _adminSvc.GetResourceCheckinsForDisaster(model.SelectedDisaster, model.CommitmentDate).ToList();
                result.OrganizationContacts = _adminSvc.GetContactsForDisaster(model.SelectedDisaster).ToList();

                var volunteers = _adminSvc.GetVolunteersForDisaster(model.SelectedDisaster, model.CommitmentDate);

                if (model.CommitmentDate == null)
                {
                    result.VolunteerCheckins = (from person in volunteers
                                                select new Person
                                                {
                                                    Commitments = person.Commitments.Where(x => x.DisasterId == model.SelectedDisaster).ToList(),
                                                    Email = person.Email,
                                                    FirstName = person.FirstName,
                                                    Organization = person.Organization,
                                                    OrganizationId = person.OrganizationId,
                                                    Id = person.Id,
                                                    LastName = person.LastName,
                                                    PhoneNumber = person.PhoneNumber,
                                                    UserId = person.UserId
                                                }).ToList();

                }
                else
                {
                    result.VolunteerCheckins = (from person in volunteers
                                                select new Person
                                                {
                                                    Commitments = person.Commitments.Where(x => x.DisasterId == model.SelectedDisaster
                                                        && model.CommitmentDate >= x.StartDate
                                                        && model.CommitmentDate <= x.EndDate).ToList(),
                                                    Email = person.Email,
                                                    FirstName = person.FirstName,
                                                    Id = person.Id,
                                                    LastName = person.LastName,
                                                    PhoneNumber = person.PhoneNumber,
                                                    UserId = person.UserId
                                                }).ToList();
                }
            }
            return PartialView("_FilterResults", result);
        }

        [HttpPost]
        public PartialViewResult FilterResourceCheckins(ListByDisasterViewModel model)
        {
            var result = new CheckinListsResultsViewModel();

            if (model.SelectedDisaster != 0)
            {
                result.ResourceCheckins = _adminSvc.GetResourceCheckinsForDisaster(model.SelectedDisaster, model.CommitmentDate).ToList();
            }

            return PartialView("_FilterResourceCheckinResults", result);
        }
    }
}
