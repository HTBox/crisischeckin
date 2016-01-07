using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class ClusterController : BaseController
    {
        private readonly ICluster _clusterSvc;
        private readonly IClusterGroup _clusterGroupSvc;

        public ClusterController(ICluster clusterSvc, IClusterGroup clusterGroupSvc)
        {
            _clusterSvc = clusterSvc;
            _clusterGroupSvc = clusterGroupSvc;
        }


        // GET: Cluster Create
        public ActionResult Create()
        {
            Cluster newCluster = new Cluster();
            ViewBag.ClusterGroups = _clusterGroupSvc.GetList();
            return View(newCluster);
        }

        // POST: Cluster/Create
        [HttpPost]
        public ActionResult Create(Cluster cluster)
        {
            if (ModelState.IsValid)
            {
                bool clustedExists = _clusterSvc.GetList().Any(t => t.Name.Equals(cluster.Name, StringComparison.CurrentCultureIgnoreCase));

                if (!clustedExists)
                {

                    if (cluster.Id == 0)
                        _clusterSvc.Create(cluster);
                    else
                        _clusterSvc.Update(cluster);

                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("Name", string.Format("The name '{0}' is already in use.", cluster.Name));
                }
            }

            ViewBag.ClusterGroups = _clusterGroupSvc.GetList();

            return View(cluster);
        }

        private static ClusterViewModel CreateViewModel(Cluster cluster)
        {
            return new ClusterViewModel
            {
                Id = cluster.Id,
                Name = cluster.Name
            };
        }

        // GET: Cluster Delete
        public ActionResult Delete(Cluster cluster)
        {
            Cluster removeCluster = _clusterSvc.Get(cluster.Id);
            _clusterSvc.Remove(removeCluster);

            return RedirectToAction("List");
        }

        // GET: Clusters/
        public ActionResult List()
        {
            IEnumerable<ClusterViewModel> viewData = _clusterSvc.GetList()
                .Select(CreateViewModel);

            return View(viewData);
        }

        // GET: Cluster Update
        public ActionResult Update(int id)
        {
            Cluster editedCluster = _clusterSvc.Get(id);

            ViewBag.ClusterGroups = _clusterGroupSvc.GetList();

            return View("Create", editedCluster);
        }

        // POST: Cluster/Update
        [HttpPost]
        public ActionResult Update(Cluster cluster)
        {
            if (ModelState.IsValid)
            {
                bool clustedExists = _clusterSvc.GetList().Any(t => t.Name.Equals(cluster.Name, StringComparison.CurrentCultureIgnoreCase));

                if (!clustedExists)
                {
                    _clusterSvc.Update(cluster);

                    return View("List", _clusterSvc.GetList().Select(CreateViewModel));
                }
                else
                {
                    ModelState.AddModelError("Name", string.Format("The name '{0}' is already in use.", cluster.Name));
                }
                
            }

            ViewBag.ClusterGroups = _clusterGroupSvc.GetList();

            return View("Create", cluster);
        }
    }
}