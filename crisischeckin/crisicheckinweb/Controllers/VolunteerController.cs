using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : BaseController
    {
        private IDisaster _disasterSvc;
        private ICluster _clusterSvc;
        private IAdmin _adminSvc;
        public VolunteerController(IDisaster disasterSvc, ICluster clusterService, IAdmin adminSvc)
        {
            _disasterSvc = disasterSvc;
            _clusterSvc = clusterService;
            _adminSvc = adminSvc;
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
            this.PopulateSendMessageViewModel(model);
            // Default subject to disaster name so they can type just a message if they want to.
            model.Subject = model.DisasterName;
            return View("CreateMessage", model);
        }

        private void PopulateSendMessageViewModel(SendMessageToAllVolunteersByDisasterViewModel model)
        {
            var disaster = _disasterSvc.Get(model.DisasterId);
            model.DisasterName = disaster.Name;
            model.Clusters = this._clusterSvc.GetList();
        }

        [HttpPost]
        public ActionResult SendMessageToVolunteersByDisaster(SendMessageToAllVolunteersByDisasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.PopulateSendMessageViewModel(model);
                return this.View("CreateMessage", model);
            }
            throw new NotImplementedException();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Filter(ListByDisasterViewModel model)
        {
            if (model.SelectedDisaster != 0)
            {

                var disaster = _disasterSvc.Get(model.SelectedDisaster);
                var results = _adminSvc.GetVolunteersForDate(disaster,
                                                             model.CommitmentDate.HasValue
                                                                 ? model.CommitmentDate.Value
                                                                 : DateTime.MinValue);
                return PartialView("_FilterResults", results);
            }
            else
            {
                return (PartialView("_FilterResults"));
            }
        }
    }
}
