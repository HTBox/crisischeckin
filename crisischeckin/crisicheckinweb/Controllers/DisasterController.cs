using System;
using System.Web.Mvc;
using Models;
using Services.Interfaces;
using Services.Exceptions;

namespace crisicheckinweb.Controllers
{
    [Authorize(Roles = Common.Constants.RoleAdmin)]
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
            int disasterId;
            var validId = int.TryParse(id, out disasterId);

            if (validId && disasterId != -1)
            {
                return View("Create", _disasterSvc.Get(disasterId));
            }
            return View("Create", new Disaster { IsActive = true });
        }

        [HttpPost]
        public ActionResult Create(Disaster disaster)
        {
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(disaster.Name))
            {
                if (disaster.Id == -1)
                {
                    try
                    {
                        _disasterSvc.Create(disaster);
                    }
                    catch (DisasterAlreadyExistsException)
                    {
                        ModelState.AddModelError("Name", "A Disaster already exists with that Name!");
                        return View("Create", disaster);
                    }
                }
                else
                {
                    try
                    {
                        _disasterSvc.Update(disaster.Id, disaster.Name, disaster.IsActive);
                    }
                    catch (DisasterAlreadyExistsException)
                    {
                        ModelState.AddModelError("Name", "A Disaster already exists with that Name!");
                        return View("Create", disaster);
                    }
                }

                return Redirect("/Disaster/List");
            }
            ModelState.AddModelError("Name", "Disaster Name is required!");
            return View("Create", disaster);
        }

        //TODO: Need to set a schedule for removal.
        [HttpPost]
        [Obsolete("POST /Edit is deprecated. Use POST /Create instead")]
        public ActionResult Edit(Disaster disaster)
        {
            TempData["EditUrlDeprecatedWarning"] = "POST /Edit is deprecated. Use POST /Create instead";
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(disaster.Name))
            {
                if (disaster.Id == -1)
                {
                    try
                    {
                        _disasterSvc.Create(disaster);
                    }
                    catch (DisasterAlreadyExistsException)
                    {
                        ModelState.AddModelError("Name", "A Disaster already exists with that Name!");
                        return View("Create", disaster);
                    }
                }
                else
                {
                    _disasterSvc.Update(disaster.Id, disaster.Name, disaster.IsActive);
                }

                return Redirect("/Disaster/List");
            }


            ModelState.AddModelError("Name", "Disaster Name is required!");
            return View("Create", disaster);
        }


        #region api methods
        public JsonResult GetActiveDisasters()
        {
            return Json(_disasterSvc.GetActiveList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
