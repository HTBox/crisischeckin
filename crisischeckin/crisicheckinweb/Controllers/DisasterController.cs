using System;
using System.Linq;
using System.Web.Mvc;
using crisicheckinweb.Filters;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;
using Services.Exceptions;

namespace crisicheckinweb.Controllers
{
    [AccessDeniedAuthorize(Roles = Common.Constants.RoleAdmin, AccessDeniedViewName = "~/Home/AccessDenied")]
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
            var viewData = _disasterSvc.GetList()
                .Select(CreateViewModel);

            return View(viewData);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int disasterId;
            var validId = int.TryParse(id, out disasterId);

            if (validId && disasterId != -1)
            {
                var disaster = _disasterSvc.Get(disasterId);
                return View("Create", CreateViewModel(disaster));
            }
            return View("Create", new DisasterViewModel { IsActive = true });
        }

        [HttpPost]
        public ActionResult Create(DisasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var disaster = new Disaster
                    {
                        Id = model.Id,
                        Name = model.Name,
                        IsActive = model.IsActive
                    };

                    if (model.Id == -1)
                    {
                        _disasterSvc.Create(disaster);
                    }
                    else
                    {
                        _disasterSvc.Update(disaster);
                    }
                }
                catch (DisasterAlreadyExistsException)
                {
                    ModelState.AddModelError("Name", "A Disaster already exists with that Name!");
                    return View("Create", model);
                }

                return Redirect("/Disaster/List");
            }
            return View("Create", model);
        }

        //TODO: Need to set a schedule for removal.
        [HttpPost]
        [Obsolete("POST /Edit is deprecated. Use POST /Create instead")]
        public ActionResult Edit(DisasterViewModel model)
        {
            TempData["EditUrlDeprecatedWarning"] = "POST /Edit is deprecated. Use POST /Create instead";
            return Create(model);
        }

        private static DisasterViewModel CreateViewModel(Disaster disaster)
        {
            return new DisasterViewModel
            {
                Id = disaster.Id,
                Name = disaster.Name,
                IsActive = disaster.IsActive
            };
        }


        #region api methods
        public JsonResult GetActiveDisasters()
        {
            return Json(_disasterSvc.GetActiveList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
