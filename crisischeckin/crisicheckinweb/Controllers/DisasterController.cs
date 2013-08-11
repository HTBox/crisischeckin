using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class DisasterController : BaseController
    {
        private readonly IDisaster _disasterSvc;
        public DisasterController(IDisaster disasterSvc)
        {
            _disasterSvc = disasterSvc;
        }

        //
        // GET: /Disaster/
        public ActionResult List()
        {
            var viewData = _disasterSvc.GetList(); 

            return View(viewData);
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            bool validId = false;
            int disasterId = 0;

            Disaster viewData;

            validId = int.TryParse(id, out disasterId);

            if (validId && disasterId != -1)
            {
                viewData = _disasterSvc.Get(disasterId);
            }
            else
            {
                // Adding new Disaster here
                viewData = new Disaster();
                viewData.IsActive = true;
            }
            
            return View(viewData);
        }

        [HttpPost]
        public RedirectResult Edit(Disaster disaster)
        {
            if (disaster.Id == -1)
            {
                _disasterSvc.Create(disaster);
            }
            else
            {
                _disasterSvc.Update(disaster.Id,disaster.Name, disaster.IsActive);
            }

            return Redirect("/Disaster/List");
        }

        #region api methods
        public JsonResult GetActiveDisasters()
        {
            return Json(_disasterSvc.GetActiveList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
