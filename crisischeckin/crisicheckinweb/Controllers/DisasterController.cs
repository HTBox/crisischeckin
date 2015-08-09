using crisicheckinweb.Filters;
using crisicheckinweb.ViewModels;
using Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace crisicheckinweb.Controllers
{
    [AccessDeniedAuthorize(Roles = Common.Constants.RoleAdmin, AccessDeniedViewName = "~/Home/AccessDenied")]
    public class DisasterController : BaseController
    {
        private readonly IDisaster _disasterSvc;
        private readonly ICluster _clusterSvc;
        private readonly IDisasterClusterService _disasterClusterSvc;

        public DisasterController(IDisaster disasterSvc, ICluster clusterSvc, IDisasterClusterService disasterClusterSvc)
        {
            _disasterSvc = disasterSvc;
            _clusterSvc = clusterSvc;
            _disasterClusterSvc = disasterClusterSvc;
        }

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
            return View("Create", new DisasterViewModel
            {
                IsActive = true,
                SelectedDisasterClusters = (from list in _clusterSvc.GetList()
                                            select new SelectedDisasterCluster 
                                            { Name = list.Name, Id = list.Id, Selected = true }).ToList()
            });
        }

        [HttpPost]
        public ActionResult Create(DisasterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.SelectedDisasterClusters.Exists(x => x.Selected == true))
                    {
                        ModelState.AddModelError("Name", "You have to include at least one Cluster!");
                        return View("Create", model);
                    }

                    var disaster = new Disaster
                    {
                        Id = model.Id,
                        Name = model.Name,
                        IsActive = model.IsActive
                    };

                    if (model.Id == -1)
                    {
                        _disasterSvc.Create(disaster);

                        var id = _disasterSvc.GetList().Where(x => x.Name == model.Name).FirstOrDefault().Id;

                        foreach (var item in model.SelectedDisasterClusters)
                        {
                            if (item.Selected == true)
                            {
                                _disasterClusterSvc.Create(new DisasterCluster { Id = -1, DisasterId = id, ClusterId = item.Id });
                            }
                        }
                    }
                    else
                    {
                        _disasterSvc.Update(disaster);

                        var disasterClusterList = _disasterClusterSvc.GetClustersForADisaster(model.Id);


                        foreach (var item in model.SelectedDisasterClusters)
                        {
                            if (item.Selected == true)
                            {
                                if(!disasterClusterList.Exists(x => x.ClusterId == item.Id))
                                {
                                    _disasterClusterSvc.Create(new DisasterCluster { Id = -1, DisasterId = model.Id, ClusterId = item.Id });
                                }
                            }
                            else
                            {
                                if (disasterClusterList.Exists(x => x.ClusterId == item.Id))
                                {
                                    _disasterClusterSvc.Remove(disasterClusterList.Find(x => x.ClusterId == item.Id));
                                }
                            }
                        }
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

        private DisasterViewModel CreateViewModel(Disaster disaster)
        {
            return new DisasterViewModel
            {
                Id = disaster.Id,
                Name = disaster.Name,
                IsActive = disaster.IsActive,
                SelectedDisasterClusters =
                    (from clustList in _clusterSvc.GetList()
                     join disList in _disasterClusterSvc.GetClustersForADisaster(disaster.Id)
                     on clustList.Id equals disList.ClusterId into outerList
                     from disList in outerList.DefaultIfEmpty()
                     select new SelectedDisasterCluster 
                     { Name = clustList.Name, Id = clustList.Id, Selected = (disList != null)}).ToList()
            };
        }

        #region api methods

        public JsonResult GetActiveDisasters()
        {
            return Json(_disasterSvc.GetActiveList(), JsonRequestBehavior.AllowGet);
        }

        #endregion api methods
    }
}