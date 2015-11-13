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

        public ClusterController(ICluster clusterSvc)
        {
            _clusterSvc = clusterSvc;
        }


        // GET: Cluster Create
        public ActionResult Create()
        {
            Cluster newCluster = new Cluster();
            return View(newCluster);
        }

        // POST: Cluster/Create
        [HttpPost]
        public ActionResult Create(Cluster cluster)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(cluster.Name))
                {
                    ModelState.AddModelError("Name", "Cluster name has to be set!");
                    return View("Create", cluster);
                }

                bool clustedExists = _clusterSvc.GetList().Any(t => t.Name.Equals(cluster.Name, StringComparison.CurrentCultureIgnoreCase));

                if (!clustedExists)
                {

                    if (cluster.Id == 0)
                        _clusterSvc.Create(cluster);
                    else
                        _clusterSvc.Update(cluster);

                    return View("List", _clusterSvc.GetList().Select(CreateViewModel));
                }
                else
                    ModelState.AddModelError("Name", string.Format("The name '{0}' is already in use.", cluster.Name));
            }
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

            return View("List", _clusterSvc.GetList().Select(CreateViewModel));
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

            return View("Create", editedCluster);
        }

        // POST: Cluster/Update
        [HttpPost]
        public ActionResult Update(Cluster cluster)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(cluster.Name))
                {
                    ModelState.AddModelError("Name", "Cluster name has to be set!");
                    return View("Create", cluster);
                }

                _clusterSvc.Update(cluster);
                return View("List", _clusterSvc.GetList().Select(CreateViewModel));
            }

            return View("Create", cluster);
        }
    }
}