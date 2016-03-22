using Common;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace crisicheckinweb.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDisaster _disasterSvc;
        private readonly IVolunteerService _volunteerSvc;
        private readonly IAdmin _adminService;
        private readonly IWebSecurityWrapper _webSecurity;
        private readonly IClusterCoordinatorService _clusterCoordinatorService;
        private readonly IVolunteerTypeService _volunteerTypes;
        private readonly IDisasterClusterService _disasterClusterSvc;

        public HomeController(
            IDisaster disasterSvc,
            IVolunteerService volunteerSvc,
            IWebSecurityWrapper webSecurity,
            IClusterCoordinatorService clusterCoordinatorService,
            IVolunteerTypeService volunteerTypeService,
            IDisasterClusterService disasterClusterService,
            IAdmin adminService
            )
        {
            _disasterSvc = disasterSvc;
            _volunteerSvc = volunteerSvc;
            _webSecurity = webSecurity;
            _clusterCoordinatorService = clusterCoordinatorService;
            _volunteerTypes = volunteerTypeService;
            _disasterClusterSvc = disasterClusterService;
            _adminService = adminService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (_webSecurity.IsUserInRole(Constants.RoleAdmin))
            {
                return RedirectToAction("List", "Disaster");
            }
            return View(GetDefaultViewModel());
        }

        [HttpPost]
        public ActionResult Checkin(int commitmentId)
        {
            return UpdateCommitment(commitmentId, c => c.CheckIn());
        }

        [HttpPost]
        public ActionResult ReportDelay(int commitmentId, DateTime startDate, DateTime endDate)
        {
            return UpdateCommitment(commitmentId, c => c.ReportDelay(startDate, endDate));
        }

        [HttpPost]
        public ActionResult ReportUnavailable(int commitmentId)
        {
            return UpdateCommitment(commitmentId, c => c.ReportUnavailable());
        }

        [HttpPost]
        public ActionResult Checkout(int commitmentId)
        {
            return UpdateCommitment(commitmentId, c => c.CheckOut());
        }

        public ActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CheckinResource(VolunteerViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", GetDefaultViewModel(model));

            try
            {
                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
                if (person == null)
                {
                    throw new ArgumentException(
                        "The logged in user is either the administrator or does not have a valid account for joining a crisis.");
                }
                _disasterSvc.AddResourceCheckIn(person.Organization, model.SelectedDisasterId, model.Description,
                    model.Qty, model.SelectedResourceTypeId, model.ResourceStartDate, model.ResourceEndDate, model.Location);

                return Redirect("/Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var modelToReturn = GetDefaultViewModel();
            modelToReturn.SelectedDisasterId = model.SelectedDisasterId;
            modelToReturn.SelectedStartDate = model.SelectedStartDate;
            modelToReturn.SelectedEndDate = model.SelectedEndDate;
            modelToReturn.SelectedClusterId = model.SelectedClusterId;
            modelToReturn.Description = model.Description;
            modelToReturn.Qty = model.Qty;
            modelToReturn.SelectedResourceTypeId = model.SelectedResourceTypeId;
            modelToReturn.ResourceStartDate = model.ResourceStartDate;
            modelToReturn.ResourceEndDate = model.ResourceEndDate;
            modelToReturn.Location = model.Location;

            if (model.SelectedDisasterId != 0)
            {
                modelToReturn.DisasterClusters
                    = _disasterClusterSvc.GetClustersForADisaster(model.SelectedDisasterId);
            }

            return View("Index", modelToReturn);
        }

        [HttpPost]
        public ActionResult Assign(VolunteerViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", GetDefaultViewModel(model));

            try
            {
                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
                if (person == null)
                {
                    throw new ArgumentException(
                        "The logged in user is either the administrator or does not have a valid account for joining a crisis.");
                }
                _disasterSvc.AssignToVolunteer(model.SelectedDisasterId,
                    person.Id, model.SelectedStartDate, model.SelectedEndDate, model.VolunteerType,
                    model.SelectedClusterId, model.Location);

                return Redirect("/Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var modelToReturn = GetDefaultViewModel();
            modelToReturn.SelectedDisasterId = model.SelectedDisasterId;
            modelToReturn.SelectedStartDate = model.SelectedStartDate;
            modelToReturn.SelectedEndDate = model.SelectedEndDate;
            modelToReturn.SelectedClusterId = model.SelectedClusterId;

            if (model.SelectedDisasterId != 0)
            {
                modelToReturn.DisasterClusters
                    = _disasterClusterSvc.GetClustersForADisaster(model.SelectedDisasterId);
            }

            return View("Index", modelToReturn);
        }

        public ActionResult LoadDisasterClusterList(int disasterId)
        {
            var disasterClusters = _disasterClusterSvc.GetClustersForADisaster(disasterId);

            return Json(disasterClusters, JsonRequestBehavior.AllowGet);
        }

        private VolunteerViewModel GetDefaultViewModel(VolunteerViewModel viewModel = null)
        {
            var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);

            var commitments = (person != null) ?
                _volunteerSvc.RetrieveCommitments(person.Id, true) :
                new List<Commitment>().AsEnumerable();

            var resources = (person != null && person.OrganizationId.HasValue) ?
                _adminService.GetResourceCheckinsForOrganization(person.OrganizationId.Value) :
                new List<Resource>().AsEnumerable();

            var resourceTypes = _adminService.GetResourceTypes();

            var commitmentForToday = commitments.FirstOrDefault(x => x.StartDate <= DateTime.Today && DateTime.Today <= x.EndDate);

            var clusterCoordinators = (commitmentForToday != null && commitmentForToday.ClusterId.HasValue) ?
                 _clusterCoordinatorService.GetAllCoordinatorsForCluster(commitmentForToday.ClusterId.Value).ToList() :
                new List<ClusterCoordinator>().AsEnumerable();


            List<AvailableAction> availableActions = new List<AvailableAction>();
            if (commitmentForToday != null)
            {
                if (commitmentForToday.Status != CommitmentStatus.Here)
                {
                    availableActions.Add(new AvailableAction
                    {
                        ActionName = "Checkin",
                        ButtonText = "Check-in",
                        Description = " and start helping now!",
                        ButtonId = "check-in-button"
                    });
                }
                if (commitmentForToday.Status == CommitmentStatus.Planned)
                {
                    availableActions.Add(new AvailableAction
                    {
                        ActionName = "ReportDelay",
                        ButtonText = "Delayed",
                        Description = " I'll be there when I can.",
                        ButtonId = "delayed-button",
                        FormId = "delayed-form"
                    });
                }
                if (commitmentForToday.Status == CommitmentStatus.Here)
                {
                    availableActions.Add(new AvailableAction
                    {
                        ActionName = "Checkout",
                        ButtonText = "Check-out",
                        Description = " Thank you for your help today!",
                        ButtonId = "check-out-button"
                    });
                }
                if (commitmentForToday.Status != CommitmentStatus.Unavailable)
                {
                    availableActions.Add(new AvailableAction
                    {
                        ActionName = "ReportUnavailable",
                        ButtonText = "Unavailable",
                        Description = " I can't help you at this time.",
                        ButtonId = "unavailable-button"
                    });
                }
            }

            var model = new VolunteerViewModel
            {
                Disasters = _disasterSvc.GetActiveList(),
                DisasterClusters = _disasterClusterSvc.GetClustersForADisaster(0),
                MyCommitments = commitments,
                MyOrgResources = resources,
                ResourceTypes = resourceTypes,
                AvailableActions = availableActions,
                VolunteerTypes = _volunteerTypes.GetList(),
                Person = person,
                ClusterCoordinators = clusterCoordinators
            };

            if (viewModel != null)
            {
                model.SelectedDisasterId = viewModel.SelectedDisasterId;
                model.SelectedStartDate = viewModel.SelectedStartDate;
                model.SelectedEndDate = viewModel.SelectedEndDate;
                model.SelectedClusterId = viewModel.SelectedClusterId;

                model.Description = viewModel.Description;
                model.Qty = viewModel.Qty;
                model.SelectedResourceTypeId = viewModel.SelectedResourceTypeId;
                model.ResourceStartDate = viewModel.ResourceStartDate;
                model.ResourceEndDate = viewModel.ResourceEndDate;

                if (model.SelectedDisasterId != 0)
                {
                    model.DisasterClusters
                        = _disasterClusterSvc.GetClustersForADisaster(model.SelectedDisasterId);
                }
            }

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveResource(VolunteerViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", GetDefaultViewModel(model));

            try
            {
                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
                if (!person.OrganizationId.HasValue)
                {
                    throw new ArgumentException("Signed in User is not part of an Organization");
                }

                var resources = _adminService.GetResourceCheckinsForOrganization(person.OrganizationId.Value);

                var resource = resources.FirstOrDefault(r => r.ResourceId == model.RemoveResourceId);
                if (resource == null)
                {
                    throw new ArgumentException("Resource supplied is not yours.");
                }

                _disasterSvc.RemoveResourceById(resource.ResourceId);

                return Redirect("/Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var modelToReturn = GetDefaultViewModel();
            modelToReturn.SelectedDisasterId = model.SelectedDisasterId;
            modelToReturn.SelectedStartDate = model.SelectedStartDate;
            modelToReturn.SelectedEndDate = model.SelectedEndDate;
            modelToReturn.SelectedClusterId = model.SelectedClusterId;

            return View("Index", modelToReturn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCommitment(VolunteerViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", GetDefaultViewModel(model));

            try
            {
                var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
                var commitments = _volunteerSvc.RetrieveCommitments(person.Id, true).AsEnumerable();


                var commitment = commitments.FirstOrDefault(c => c.Id == model.RemoveCommitmentId);
                if (commitment == null)
                {
                    throw new ArgumentException("Commitment supplied is not yours.");
                }

                _disasterSvc.RemoveClusterCoordinator(_webSecurity.CurrentUserId, commitment.ClusterId.Value, commitment.DisasterId);
                _disasterSvc.RemoveCommitmentById(model.RemoveCommitmentId);
                

                return Redirect("/Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var modelToReturn = GetDefaultViewModel();
            modelToReturn.SelectedDisasterId = model.SelectedDisasterId;
            modelToReturn.SelectedStartDate = model.SelectedStartDate;
            modelToReturn.SelectedEndDate = model.SelectedEndDate;
            modelToReturn.SelectedClusterId = model.SelectedClusterId;

            return View("Index", modelToReturn);
        }

        private ActionResult UpdateCommitment(int commitmentId, Action<Commitment> change)
        {
            var person = _volunteerSvc.FindByUserId(_webSecurity.CurrentUserId);
            if (person != null)
            {
                var commitment = _volunteerSvc.RetrieveCommitments(person.Id, true)
                    .FirstOrDefault(x => x.Id == commitmentId);
                if (commitment != null)
                {
                    change(commitment);
                    _volunteerSvc.UpdateCommitment(commitment);
                }
            }

            return RedirectToAction("Index");
        }
    }
}