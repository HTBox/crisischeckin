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
            if (!ModelState.IsValid) return View("Index", GetDefaultViewModel(model));
                
            try
            {
	            if (DateTime.Compare(DateTime.Today, model.SelectedStartDate) > 0)
	            {
                    throw new ArgumentException("Please enter a start date that is greater than today's date.");
                }

                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
				if (person == null)
				{
					throw new ArgumentException("The logged in user is either the administrator or does not have a valid account for joining a crisis.");
				}
                _disasterSvc.AssignToVolunteer(model.SelectedDisasterId,
                    person.Id, model.SelectedStartDate, model.SelectedEndDate);

                return Redirect("/Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var modelToReturn = GetDefaultViewModel();
            modelToReturn.SelectedDisasterId = model.SelectedDisasterId;
            modelToReturn.SelectedStartDate = model.SelectedStartDate;
            modelToReturn.SelectedEndDate = model.SelectedEndDate;

            return View("Index", modelToReturn);
        }

        private VolunteerViewModel GetDefaultViewModel(VolunteerViewModel viewModel = null)
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
            if (viewModel != null)
            {
                model.SelectedDisasterId = viewModel.SelectedDisasterId;
                model.SelectedStartDate = viewModel.SelectedStartDate;
                model.SelectedEndDate = viewModel.SelectedEndDate;
            }

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCommitment(VolunteerViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", GetDefaultViewModel(model));

            try
            {
                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
                var commitments = _volunteerSvc.RetrieveCommitments(person.Id, true).AsEnumerable();

                if (commitments.Where(c => c.Id == model.RemoveCommitmentId).FirstOrDefault() == null)
                {
                    throw new ArgumentException("Commitment supplied is not yours.");
                }

                _disasterSvc.RemoveCommitmentById(model.RemoveCommitmentId);

                return Redirect("/Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var modelToReturn = GetDefaultViewModel();
            modelToReturn.SelectedDisasterId = model.SelectedDisasterId;
            modelToReturn.SelectedStartDate = model.SelectedStartDate;
            modelToReturn.SelectedEndDate = model.SelectedEndDate;

            return View("Index", modelToReturn);
        }

    }
}
