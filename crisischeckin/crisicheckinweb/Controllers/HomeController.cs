using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;
using crisicheckinweb.Wrappers;

namespace crisicheckinweb.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IDisaster _disasterSvc;
        private readonly IVolunteerService _volunteerSvc;
        private readonly IWebSecurityWrapper _webSecurity;

        public HomeController(IDisaster disasterSvc, IVolunteerService volunteerSvc, IWebSecurityWrapper webSecurity)
        {
            _disasterSvc = disasterSvc;
            _volunteerSvc = volunteerSvc;
            _webSecurity = webSecurity;
        }

        // GET: /Home/
        public ActionResult Index()
        {
            return View(GetDefaultViewModel());
        }

        [HttpPost]
        public ActionResult Assign(VolunteerViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", GetDefaultViewModel());
                
            try
            {
	            if (DateTime.Compare(DateTime.Today, model.SelectedStartDate) > 0)
	            {
	                throw new ArgumentException("Please enter a start date that is greater than today's date.");
                }

                Person me = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
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

        private VolunteerViewModel GetDefaultViewModel()
        {
            var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
            var commitments = (person != null) ?
                _volunteerSvc.RetrieveCommitments(person.Id, true) :
                new List<Commitment>().AsEnumerable();

            var model = new VolunteerViewModel
            {
                Disasters = _disasterSvc.GetActiveList(),
                MyCommitments = commitments
            };

            return model;
        }

    }
}
