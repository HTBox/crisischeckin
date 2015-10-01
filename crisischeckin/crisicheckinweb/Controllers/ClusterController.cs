using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;
using crisicheckinweb.ViewModels;
using System.Web.Mvc;


namespace crisicheckinweb.Controllers
{
    public class ClusterController : BaseController
    {
        private readonly ICluster _clusterSvc;

        public ClusterController(ICluster clusterSvc)
        {
            _clusterSvc = clusterSvc;
        }

        // GET: Clusters/
        public ActionResult List()
        {
            var viewData = _clusterSvc.GetList()
                .Select(CreateViewModel);

            return View(viewData);
        }

  
        // GET: Cluster Create
        public ActionResult Create()
        {

            var newCluster = new Cluster();
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

                var newCluster = new Cluster
                {
                    Name = cluster.Name,
                };

                if (cluster.Id == 0)
                {
                    _clusterSvc.Create(cluster);
                }
                else
                {
                    _clusterSvc.Update(cluster);
                }

                return View("List", _clusterSvc.GetList().Select(CreateViewModel));
            }
            return View(cluster);
        }

        // GET: Cluster Update
        public ActionResult Update(int id)
        {
           var editedCluster = _clusterSvc.Get(id);

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


        // GET: Cluster Delete
        public ActionResult Delete(Cluster cluster)
        {
            var removeCluster = _clusterSvc.Get(cluster.Id);
            _clusterSvc.Remove(removeCluster);

            return View("List", _clusterSvc.GetList()
                .Select(CreateViewModel));
        }



        private static ClusterViewModel CreateViewModel(Cluster cluster)
        {
            return new ClusterViewModel
            {
                Id = cluster.Id,
                Name = cluster.Name
            };
        }

    }

}