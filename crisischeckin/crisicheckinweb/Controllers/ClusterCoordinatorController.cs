using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class ClusterCoordinatorController : Controller
    {
        readonly ICluster _cluster;
        readonly IClusterCoordinatorService _clusterCoordinatorService;
        readonly IDataService _dataService;
        readonly IDisaster _disaster;

        public ClusterCoordinatorController(
            IDisaster disaster,
            ICluster cluster,
            IClusterCoordinatorService clusterCoordinatorService,
            IDataService dataService)
        {
            _dataService = dataService;
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
            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinators(disaster.Id);
            var disasterClusterCoordinatorsViewModel =
                new DisasterClusterCoordinatorsViewModel
                {
                    DisasterName = disaster.Name,
                    DisasterId = disaster.Id,
                    Clusters = _cluster
                        .GetList()
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
                    AvailableClusters = _cluster.GetList().ToList(),
                    AvailablePeople = _dataService.Persons.ToList(),
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
            var disasterClusterCoordinatorsViewModel = GetDisasterClusterCoordinatorsViewModel(_disaster.Get(disasterId));
            return PartialView(disasterClusterCoordinatorsViewModel);
        }

        [HttpGet]
        public ActionResult ConfirmUnassignCoordinator(int id)
        {
            var clusterCoordinator = _clusterCoordinatorService.GetCoordinator(id);
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
            var clusterCoordinator = _clusterCoordinatorService.GetCoordinator(id);
            if (null == clusterCoordinator)
                return RedirectToAction("Index", "Home");
            _clusterCoordinatorService.UnassignClusterCoordinator(clusterCoordinator);
            return RedirectToAction("Index", new { id = clusterCoordinator.DisasterId });
        }
    }
}