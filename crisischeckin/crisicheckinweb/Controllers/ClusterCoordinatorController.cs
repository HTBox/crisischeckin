using System.Linq;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class ClusterCoordinatorController : Controller
    {
        readonly IDataService _dataService;
        readonly ICluster _cluster;
        readonly IClusterCoordinatorService _clusterCoordinatorService;
        readonly IDisaster _disaster;

        public ClusterCoordinatorController(IDisaster disaster, ICluster cluster, IClusterCoordinatorService clusterCoordinatorService, IDataService dataService)
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
            {
                return RedirectToAction("List", "Disaster");
            }

            var disasterClusterCoordinatorsViewModel = GetDisasterClusterCoordinatorsViewModel(id.GetValueOrDefault());
            return View(disasterClusterCoordinatorsViewModel);
        }

        DisasterClusterCoordinatorsViewModel GetDisasterClusterCoordinatorsViewModel(int disasterId)
        {
            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinators(disasterId);
            var disasterClusterCoordinatorsViewModel = new DisasterClusterCoordinatorsViewModel
                                                       {
                                                           DisasterName = _disaster.Get(disasterId).Name,
                                                           DisasterId = disasterId,
                                                           Clusters = _cluster
                                                               .GetList()
                                                               .Select(c => new ClusterViewModel
                                                                            {
                                                                                Name = c.Name,
                                                                                Coordinators = clusterCoordinators
                                                                                    .Where(x => x.ClusterId == c.Id)
                                                                                    .Select(x => new ClusterCoordinatorViewModel {Name = x.Coordinator.FullName})
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
            _clusterCoordinatorService.AssignClusterCoordinator(clusterCoordinator.DisasterId, clusterCoordinator.SelectedClusterId, clusterCoordinator.SelectedPersonId);
            var disasterClusterCoordinatorsViewModel = GetDisasterClusterCoordinatorsViewModel(clusterCoordinator.DisasterId);
            return PartialView(disasterClusterCoordinatorsViewModel);
        }
    }
}