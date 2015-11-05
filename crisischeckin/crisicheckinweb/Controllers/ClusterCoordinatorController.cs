using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{

    
    public class ClusterCoordinatorController : BaseController
    {
        readonly ICluster _cluster;
        readonly IClusterCoordinatorService _clusterCoordinatorService;
        readonly IDisaster _disaster;

        public ClusterCoordinatorController(
            IDisaster disaster,
            ICluster cluster,
            IClusterCoordinatorService clusterCoordinatorService)
        {
            _disaster = disaster;
            _cluster = cluster;
            _clusterCoordinatorService = clusterCoordinatorService;
        }

        [HttpGet]
        public ActionResult Index(int? id = -1)
        {
            if (id == -1)
                return RedirectToAction("List", "Disaster");
            var disaster = _disaster.Get(id.GetValueOrDefault());
            if (null == disaster)
                return RedirectToAction("List", "Disaster");
            var vm = GetDisasterClusterCoordinatorsViewModel(disaster);
            return View(vm);
        }

        DisasterClusterCoordinatorsViewModel GetDisasterClusterCoordinatorsViewModel(Disaster disaster)
        {
            IList<Person> allPersonDataForDisplay;
            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinatorsForDisplay(disaster.Id, out allPersonDataForDisplay);
            var allClusters = _cluster.GetList().ToList();
            var disasterClusterCoordinatorsViewModel =
                new DisasterClusterCoordinatorsViewModel
                {
                    DisasterName = disaster.Name,
                    DisasterId = disaster.Id,
                    Clusters = allClusters
                        .Select(c => new ClusterViewModel
                                     {
                                         Name = c.Name,
                                         Coordinators = clusterCoordinators
                                             .Where(x => x.ClusterId == c.Id)
                                             .Select(x => new ClusterCoordinatorViewModel
                                                          {
                                                              Name = x.Person.FullName,
                                                              Id = x.Id,
                                                          })
                                             .ToList(),
                                     })
                        .ToList(),
                    AvailableClusters = allClusters,
                    AvailablePeople = allPersonDataForDisplay
                };
            return disasterClusterCoordinatorsViewModel;
        }

        public PartialViewResult AssignCoordinator(AssignClusterCoordinatorViewModel clusterCoordinator)
        {
            _clusterCoordinatorService.AssignClusterCoordinator(
                clusterCoordinator.DisasterId,
                clusterCoordinator.SelectedClusterId,
                clusterCoordinator.SelectedPersonId);
            int disasterId = clusterCoordinator.DisasterId;
            var disasterClusterCoordinatorsViewModel = GetDisasterClusterCoordinatorsViewModel(new Disaster {Id = disasterId});
            return PartialView(disasterClusterCoordinatorsViewModel);
        }

        [HttpGet]
        public ActionResult ConfirmUnassignCoordinator(int id, int disasterId)
        {
            var clusterCoordinator = _clusterCoordinatorService.GetCoordinatorForUnassign(id);
            if (clusterCoordinator == null)
            {
                return RedirectToAction("Index", new { id = disasterId });
            }

            var vm = new UnassignClusterCoordinatorViewModel
                     {
                         DisasterId = clusterCoordinator.DisasterId,
                         CoordinatorId = id,
                         CoordinatorName = clusterCoordinator.Person.FullName,
                         ClusterName = clusterCoordinator.Cluster.Name,
                     };
            return View(vm);
        }

        [HttpPost]
        public ActionResult UnassignCoordinator(int id)
        {
            var clusterCoordinator = _clusterCoordinatorService.GetCoordinatorFullyLoaded(id);
            if (null == clusterCoordinator)
                return RedirectToAction("Index");
            _clusterCoordinatorService.UnassignClusterCoordinator(clusterCoordinator);
            return RedirectToAction("Index", new { id = clusterCoordinator.DisasterId });
        }

       
    }
}