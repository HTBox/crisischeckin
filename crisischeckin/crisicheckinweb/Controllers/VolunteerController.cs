using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : Controller
    {
        private IDisaster _disasterSvc;
        public VolunteerController(IDisaster disasterSvc)
        {
            _disasterSvc = disasterSvc;
        }

        [HttpGet]
        public ActionResult ListByDisaster()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ListByDisaster(string id, string filterDate)
        {
            return View();
        }
    }
}
