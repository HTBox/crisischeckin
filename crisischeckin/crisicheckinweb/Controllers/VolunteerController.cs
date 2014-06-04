using Services.Interfaces;
using System;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;

namespace crisicheckinweb.Controllers
{
    using Services;

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
            ViewBag.Clusters = _clusterSvc.GetList();
            var model = new SendMessageToAllVolunteersByDisasterViewModel { DisasterId = id };
            PopulateSendMessageViewModel(model);
            // Default subject to disaster name so they can type just a message if they want to.
            model.Subject = model.DisasterName;
            return View("CreateMessage", model);
        }

        private void PopulateSendMessageViewModel(SendMessageToAllVolunteersByDisasterViewModel model)
        {
            var disaster = _disasterSvc.Get(model.DisasterId);
            model.DisasterName = disaster.Name;
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
            var recipientCriterion = new RecipientCriterion(model.DisasterId, model.ClusterId);
            var message = new Message(model.Subject, model.Message);
            _messageSvc.SendMessageToDisasterVolunteers(recipientCriterion, message);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Filter(ListByDisasterViewModel model)
        {
            if (model.SelectedDisaster != 0)
            {

                var disaster = _disasterSvc.Get(model.SelectedDisaster);
                var results = model.CommitmentDate.HasValue ?
                    _adminSvc.GetVolunteersForDate(disaster, model.CommitmentDate.Value) :
                    _adminSvc.GetVolunteers(disaster);
                return PartialView("_FilterResults", results);
            }
            return PartialView("_FilterResults");
        }
    }
}
