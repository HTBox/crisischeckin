using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;
using WebMatrix.WebData;

namespace crisicheckinweb.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IDisaster _disasterSvc;
        private readonly IVolunteer _volunteerSvc;

        public HomeController(IDisaster disasterSvc, IVolunteer volunteerSvc)
        {
            _disasterSvc = disasterSvc;
            _volunteerSvc = volunteerSvc;
        }

        // GET: /Home/
        public ActionResult Index()
        {
            return View(GetDefaultViewModel());
        }

        [HttpPost]
        public ActionResult Assign(VolunteerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // validate the date entry - make sure it's not in the past and
                // that the start date is not ahead of the end date
                if (DateTime.Compare(DateTime.Today, model.SelectedStartDate) > 0)
                {
                    ModelState.AddModelError("", "Please enter a start date that is greater than today's date.");
                }

                if (DateTime.Compare(model.SelectedStartDate, model.SelectedEndDate) >= 0)
                {
                    ModelState.AddModelError("", "Start Date must come before End Date.");
                }

                // check again if the dates are valid
                if (ModelState.IsValid)
                {
                    Person me = _volunteerSvc.FindByUserId(WebSecurity.CurrentUserId);
                    _disasterSvc.AssignToVolunteer(new Disaster { Id = model.SelectedDisaster },
                        me, model.SelectedStartDate, model.SelectedEndDate);

                    return Redirect("/Home");
                }

                var modelToReturn = GetDefaultViewModel();
                modelToReturn.SelectedDisaster = model.SelectedDisaster;
                modelToReturn.SelectedStartDate = model.SelectedStartDate;
                modelToReturn.SelectedEndDate = model.SelectedEndDate;

                return View("Index", modelToReturn);
            }

            return View("Index", GetDefaultViewModel());
        }

        private VolunteerViewModel GetDefaultViewModel()
        {
            var person = _volunteerSvc.FindByUserId(WebSecurity.CurrentUserId);
            IEnumerable<Commitment> comms = (person != null) ?
                _volunteerSvc.RetrieveCommitments(person, true) :
                new List<Commitment>().AsEnumerable();

            var model = new VolunteerViewModel
            {
                Disasters = _disasterSvc.GetActiveList(),
                MyCommitments = comms
            };

            return model;
        }

    }
}
