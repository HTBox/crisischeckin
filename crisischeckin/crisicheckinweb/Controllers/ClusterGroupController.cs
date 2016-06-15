using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class ClusterGroupController : BaseController
    {
        private readonly IClusterGroup _clusterGroupSvc;

        public ClusterGroupController(IClusterGroup clusterGroupSvc)
        {
            _clusterGroupSvc = clusterGroupSvc;
        }


        // GET: ClusterGroup Create
        public ActionResult Create()
        {
            ClusterGroup newClusterGroup = new ClusterGroup();
            return View(newClusterGroup);
        }

        // POST: ClusterGroup/Create
        [HttpPost]
        public ActionResult Create(ClusterGroup clusterGroup)
        {
            if (ModelState.IsValid)
            {
                bool clustedExists = _clusterGroupSvc.GetList().Any(t => t.Name.Equals(clusterGroup.Name, StringComparison.CurrentCultureIgnoreCase));

                if (!clustedExists)
                {

                    if (clusterGroup.Id == 0)
                        _clusterGroupSvc.Create(clusterGroup);
                    else
                        _clusterGroupSvc.Update(clusterGroup);

                    return View("List", _clusterGroupSvc.GetList().Select(CreateViewModel));
                }
                else
                {
                    ModelState.AddModelError("Name", string.Format("The name '{0}' is already in use.", clusterGroup.Name));
                }
            }

            ViewBag.ClusterGroups = _clusterGroupSvc.GetList();

            return View(clusterGroup);
        }

        private static ClusterGroupViewModel CreateViewModel(ClusterGroup clusterGroup)
        {
            return new ClusterGroupViewModel
            {
                Id = clusterGroup.Id,
                Name = clusterGroup.Name,
                Description = clusterGroup.Description
            };
        }

        // GET: Cluster Group Delete
        public ActionResult Delete(ClusterGroup cluster)
        {
            ClusterGroup removeCluster = _clusterGroupSvc.Get(cluster.Id);
            _clusterGroupSvc.Remove(removeCluster);

            return View("List", _clusterGroupSvc.GetList().Select(CreateViewModel));
        }

        // GET: ClusterGroups/
        public ActionResult List()
        {
            IEnumerable<ClusterGroupViewModel> viewData = _clusterGroupSvc.GetList()
                .Select(CreateViewModel);

            return View(viewData);
        }

        // GET: ClusterGroup Update
        public ActionResult Update(int id)
        {
            ClusterGroup editedCluster = _clusterGroupSvc.Get(id);

            return View("Create", editedCluster);
        }

        // POST: ClusterGroup/Update
        [HttpPost]
        public ActionResult Update(ClusterGroup cluster)
        {
            if (ModelState.IsValid)
            {
                bool clustedExists = _clusterGroupSvc.GetList().Any(t => t.Name.Equals(cluster.Name, StringComparison.CurrentCultureIgnoreCase));

                if (!clustedExists)
                {
                    _clusterGroupSvc.Update(cluster);

                    return View("List", _clusterGroupSvc.GetList().Select(CreateViewModel));
                }
                else
                {
                    ModelState.AddModelError("Name", string.Format("The name '{0}' is already in use.", cluster.Name));
                }
                
            }

            return View("Create", cluster);
        }
    }
}