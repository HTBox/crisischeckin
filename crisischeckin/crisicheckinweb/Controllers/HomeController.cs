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
                try
                {
                    Person me = _volunteerSvc.FindByUserId(WebSecurity.CurrentUserId);
                    _disasterSvc.AssignToVolunteer(new Disaster { Id = model.SelectedDisaster },
                        me, model.SelectedStartDate, model.SelectedEndDate);

                    return Redirect("/Home");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
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
