using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Services.Interfaces;

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
            //var model = new ListByDisasterViewModel { Disasters = _disasterSvc.GetActiveList() };
            var model = new VolunteerViewModel { Disasters = _disasterSvc.GetActiveList() };
            return View(model);
        }

    }
}
