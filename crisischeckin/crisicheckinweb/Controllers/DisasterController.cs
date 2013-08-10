using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crisicheckinweb.Controllers
{
    public class DisasterController : Controller
    {
        //
        // GET: /Disaster/

        public ActionResult List()
        {
            return View();
        }
        
        public ActionResult Edit()
        {
            return View();
        }
    }
}
