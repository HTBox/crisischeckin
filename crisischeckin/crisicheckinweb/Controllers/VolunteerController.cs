using Services.Interfaces;
using System;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : BaseController
    {
        private readonly IDisaster _disasterSvc;
        private readonly IAdmin _adminSvc;
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
            if (model.SelectedDisaster != 0)
            {

                var disaster = _disasterSvc.Get(model.SelectedDisaster);
                var results = _adminSvc.GetVolunteersForDate(disaster,
                                                             model.CommitmentDate.HasValue
                                                                 ? model.CommitmentDate.Value
                                                                 : DateTime.MinValue);
                return PartialView("_FilterResults", results);
            }
            return PartialView("_FilterResults");
        }
    }
}
