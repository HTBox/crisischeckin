using Services;
using Services.Interfaces;
using System;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Common;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : BaseController
    {
        private readonly IDisaster _disasterSvc;
        private readonly ICluster _clusterSvc;
        private readonly IAdmin _adminSvc;
        private readonly IMessageService _messageSvc;

        public VolunteerController(IDisaster disasterSvc, ICluster clusterSvc, IAdmin adminSvc, IMessageService messageSvc)
        {
            _disasterSvc = disasterSvc;
            _clusterSvc = clusterSvc;
            _adminSvc = adminSvc;
            _messageSvc = messageSvc;
        }

        [HttpGet]
        public ActionResult ListByDisaster()
        {
            var model = new ListByDisasterViewModel { Disasters = _disasterSvc.GetActiveList() };
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
            if (model.SelectedDisaster != 0)
            {
                var results = _adminSvc.GetVolunteersForDisaster(model.SelectedDisaster, model.CommitmentDate);
                return PartialView("_FilterResults", results);
            }
            return PartialView("_FilterResults");
        }
    }
}
