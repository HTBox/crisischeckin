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
        public HomeController(IDisaster disasterSvc)
        {
            _disasterSvc = disasterSvc;
        }

        // GET: /Home/
        public ActionResult Index()
        {
            List<Commitment> comms = new List<Commitment>();
            comms.Add(new Commitment() {
                Id = 1,
                DisasterId = 1,
                PersonId = 1,
                StartDate = new DateTime(2013,01,01),
                EndDate = new DateTime(2013,04,01)
            }); 

            var model = new VolunteerViewModel { Disasters = _disasterSvc.GetActiveList(),
                MyCommitments = comms };
            return View(model);
        }

        [HttpPost]
        public RedirectResult Assign(VolunteerViewModel model)
        {

            _disasterSvc.AssignToVolunteer(new Disaster { Id = model.SelectedDisaster },
                new Person { Id = WebSecurity.CurrentUserId }, model.SelectedStartDate, model.SelectedEndDate);

            return Redirect("/Home");
        }

    }
}
