using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crisicheckinweb.Controllers
{
    public class VolunteerController : Controller
    {
     
        [HttpGet]
        public ActionResult ListByDisaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListByDisaster(string filterDate)
        {
            return View();
        }

    }
}
