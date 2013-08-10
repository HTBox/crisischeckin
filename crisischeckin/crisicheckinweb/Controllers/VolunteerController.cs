using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : Controller
    {
        private IDisaster _disasterSvc;
        private IAdmin _adminSvc;
        public VolunteerController(IDisaster disasterSvc, IAdmin adminSvc)
        {
            _disasterSvc = disasterSvc;
            _adminSvc = adminSvc;
        }

        [HttpGet]
        public ActionResult ListByDisaster()
        {
            var model = new ListByDisasterViewModel { Disasters = _disasterSvc.GetActiveList() };
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Filter(ListByDisasterViewModel model)
        {
            var disaster = _disasterSvc.Get(model.SelectedDisaster);
            var results = _adminSvc.GetVolunteers(disaster);
            return PartialView("_FilterResults", results);
        }
    }
}
