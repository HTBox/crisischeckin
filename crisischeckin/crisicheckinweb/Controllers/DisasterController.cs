using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace crisicheckinweb.Controllers
{
    public class DisasterController : Controller
    {
        //
        // GET: /Disaster/
        public ActionResult List()
        {
            var viewData = new List<Disaster>();

            for (int intI = 0; intI < 5; intI++)
            {
                var disaster = new Disaster {Id = intI, IsActive = true, Name = "Disaster Name " + intI.ToString()};
                viewData.Add(disaster);
            }

            // TODO: Pull actual List of all disasters here
            return View(viewData);
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            bool validId = false;
            int disasterId = 0;

            var viewData = new Disaster();

            validId = int.TryParse(id, out disasterId);

            if (validId)
            {
                // TODO: Pull actual disaster by ID
                viewData.Id = disasterId;
                viewData.Name = "Disaster Name " + disasterId.ToString();
                viewData.IsActive = true;
            }
            
            return View(viewData);
        }

        [HttpPost]
        public RedirectResult Edit(Disaster disaster)
        {
            // TODO: Update the disaster data by ID
            return Redirect("/Disaster/List");
        }
    }
}
