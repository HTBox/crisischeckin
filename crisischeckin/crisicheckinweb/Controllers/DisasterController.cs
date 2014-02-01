using System;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
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
            
            AddUpdateDisasterModel model = new AddUpdateDisasterModel();

            if (validId && disasterId != -1)
            {
                model.Disaster = _disasterSvc.Get(disasterId);
                model.EditMode = EditMode.Updating;
            }
            else
            {
                model.Disaster = new Disaster {Id = -1, IsActive = true};
                model.EditMode = EditMode.Creating;
            }
            
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(AddUpdateDisasterModel model)
        {
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(model.Disaster.Name))
            {
                if (model.Disaster.Id == -1)
                {
                    try
                    {
                        _disasterSvc.Create(model.Disaster);
                    }
                    catch (DisasterAlreadyExistsException)
                    {
                        ModelState.AddModelError("Name", "A Disaster already exists with that Name!");
                        return View(model);
                    }
                }
                else
                {
                    _disasterSvc.Update(model.Disaster.Id, model.Disaster.Name, model.Disaster.IsActive);
                }

                return Redirect("/Disaster/List");
            }
            ModelState.AddModelError("Name", "Disaster Name is required!");
            return View(model);
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
