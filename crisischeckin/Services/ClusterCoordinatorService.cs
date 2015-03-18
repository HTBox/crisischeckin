using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Models;
using Services.Interfaces;

namespace Services
{
    public class ClusterCoordinatorService : IClusterCoordinatorService
    {
        private readonly IDataService _dataService;

        public ClusterCoordinatorService(IDataService dataService)
        {
            this._dataService = dataService;
        }

        public ClusterCoordinator AssignClusterCoordinator(int disasterId, int clusterId, int personId)
        {
            if (clusterId == 0 || personId == 0)
                return null;

            var doesCoordinatorExist = DoesCoordinatorExist(disasterId, clusterId, personId);
            if (!doesCoordinatorExist)
                AddClusterCoordinator(disasterId, clusterId, personId);
           
            var result = _dataService.ClusterCoordinators.Include(x => x.Person).Include(x => x.Disaster).Include(x => x.Cluster)
                .Where(x => x.DisasterId == disasterId
                            && x.ClusterId == clusterId
                            && x.PersonId == personId)
                .Select(x => new
                        {
                            Id = x.Id,
                            PersonId = x.Person.Id,
                            DisasterId = x.Disaster.Id,
                            ClusterId = x.Cluster.Id,
                            FirstName = x.Person.FirstName,
                            LastName = x.Person.LastName,
                            ClusterName = x.Cluster.Name,
                            DisasterName = x.Disaster.Name
                        }).First();

            var coordinator = new ClusterCoordinator
            {
                Id = result.Id,
                PersonId = result.PersonId,
                DisasterId = result.DisasterId,
                ClusterId = result.ClusterId,
                Person = new Person
                {
                    Id = result.PersonId,
                    FirstName = result.FirstName,
                    LastName = result.LastName
                },
                Cluster = new Cluster
                {
                    Id = result.ClusterId,
                    Name = result.ClusterName
                },
                Disaster = new Disaster
                {
                    Id = result.DisasterId,
                    Name = result.DisasterName
                }
            };
            if (!doesCoordinatorExist) // if the coordinator didn't originally exist when we started (so it was added), we want to add the log entry.
                AppendLogEntry(coordinator);
            return coordinator;
        }

        private bool DoesCoordinatorExist(int disasterId, int clusterId, int personId)
        {
            return _dataService.ClusterCoordinators.Any(
                x => x.DisasterId == disasterId
                && x.ClusterId == clusterId
                && x.PersonId == personId);
        }

        ClusterCoordinator AddClusterCoordinator(int disasterId, int clusterId, int personId)
        {
            return _dataService.AddClusterCoordinator(new ClusterCoordinator
                                                     {
                                                         DisasterId = disasterId,
                                                         ClusterId = clusterId,
                                                         PersonId = personId,
                                                     });
        }

        private void AppendLogEntry(ClusterCoordinator coordinator)
        {
            var clusterCoordinatorLogEntry = new ClusterCoordinatorLogEntry
                                             {
                                                 Event = ClusterCoordinatorEvents.Assigned,
                                                 TimeStampUtc = DateTime.UtcNow,
                                                 ClusterId = coordinator.ClusterId,
                                                 ClusterName = coordinator.Cluster.Name,
                                                 DisasterId = coordinator.DisasterId,
                                                 DisasterName = coordinator.Disaster.Name,
                                                 PersonId = coordinator.PersonId,
                                                 PersonName = coordinator.Person.FullName,
                                             };
            _dataService.AppendClusterCoordinatorLogEntry(clusterCoordinatorLogEntry);
        }

        public void UnassignClusterCoordinator(ClusterCoordinator clusterCoordinator)
        {           
            var clusterCoordinatorLogEntry = new ClusterCoordinatorLogEntry
                                             {
                                                 Event = ClusterCoordinatorEvents.Unassigned,
                                                 TimeStampUtc = DateTime.UtcNow,
                                                 ClusterId = clusterCoordinator.ClusterId,
                                                 ClusterName = clusterCoordinator.Cluster.Name,
                                                 DisasterId = clusterCoordinator.DisasterId,
                                                 DisasterName = clusterCoordinator.Disaster.Name,
                                                 PersonId = clusterCoordinator.PersonId,
                                                 PersonName = clusterCoordinator.Person.FullName
                                             };
            _dataService.RemoveClusterCoordinator(clusterCoordinator);
            _dataService.AppendClusterCoordinatorLogEntry(clusterCoordinatorLogEntry);
        }

        public ClusterCoordinator GetCoordinator(int id)
        {
            return _dataService.ClusterCoordinators.SingleOrDefault(x => x.Id == id);
        }

        public ClusterCoordinator GetCoordinatorFullyLoaded(int id)
        {
            return _dataService.ClusterCoordinators.Include(x => x.Person).Include(x => x.Disaster).Include(x => x.Cluster)
                .SingleOrDefault(x => x.Id == id);
        }

        public ClusterCoordinator GetCoordinatorForUnassign(int id)
        {
            var result = (from c in _dataService.ClusterCoordinators
                          join p in _dataService.Persons
                            on c.PersonId equals p.Id
                          join cl in _dataService.Clusters
                            on c.ClusterId equals cl.Id
                          where c.Id == id
                          select new
                          {
                              Id = c.Id,
                              PersonId = c.PersonId,
                              DisasterId = c.DisasterId,
                              ClusterId = c.ClusterId,
                              FirstName = p.FirstName,
                              LastName = p.LastName,
                              ClusterName = cl.Name
                          }).FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            return new ClusterCoordinator
            {
                Id = result.Id,
                PersonId = result.PersonId,
                DisasterId = result.DisasterId,
                ClusterId = result.ClusterId,
                Person = new Person
                {
                    Id = result.PersonId,
                    FirstName = result.FirstName,
                    LastName = result.LastName
                },
                Cluster = new Cluster
                {
                    Id = result.ClusterId,
                    Name = result.ClusterName
                },
                Disaster = new Disaster
                {
                    Id = result.DisasterId
                }
            };
        }

        public IEnumerable<ClusterCoordinator> GetAllCoordinators(int disasterId)
        {
            return _dataService.ClusterCoordinators.Where(x => x.DisasterId == disasterId).ToList();
        }

        public IEnumerable<ClusterCoordinator> GetAllCoordinatorsForCluster(int clusterId)
        {
            return _dataService.ClusterCoordinators.Where(x => x.ClusterId == clusterId).ToList();
        }

        public IEnumerable<ClusterCoordinator> GetAllCoordinatorsForDisplay(int disasterId, out IList<Person> allPersonsForDisplay)
        {
            var result = (from x in _dataService.ClusterCoordinators
                          where x.DisasterId == disasterId
                          select new
                          {
                              Id = x.Id,
                              PersonId = x.PersonId,
                              ClusterId = x.ClusterId
                          }).ToList();

            var persons = GetAllPersonDataForDisasterForDisplay(disasterId);
            allPersonsForDisplay = persons;

            return result.Select(x => new ClusterCoordinator
                {
                    Id = x.Id,
                    ClusterId = x.ClusterId,
                    PersonId = x.PersonId,
                    Person = persons.FirstOrDefault(per => x.PersonId == per.Id)
                }).ToList();
        }

        private IList<Person> GetAllPersonDataForDisasterForDisplay(int disasterId)
        {
            var result = (from p in _dataService.Persons
                          join cm in _dataService.Commitments
                           on p.Id equals cm.PersonId
                          where cm.DisasterId == disasterId
                          select new
                            {
                                Id = p.Id,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                ClusterId = p.ClusterId
                            }).ToList();
            return result.Select(p => new Person
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    ClusterId = p.ClusterId
                }).ToList();
        }
    }
}