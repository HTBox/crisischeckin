using System;
using System.Linq;
using Models;
using Services;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    public class DataAccessHelper
    {
        readonly DataService _dataService;

        public DataAccessHelper(DataService dataService)
        {
            _dataService = dataService;
        }

        public Person Create_a_volunteer()
        {
            var volunteerService = new VolunteerService(_dataService);
            return volunteerService.Register("Sally", "Struthers", null, "sally@struthers.com", "890-1230-4567", 100);
        }

        public Disaster Create_a_disaster()
        {
            var disaster = new Disaster
                           {
                               IsActive = true,
                               Name = "Great Seattle Starbucks Strike",
                           };
            _dataService.AddDisaster(disaster);
            return disaster;
        }

        public int GetRandomClusterId()
        {
            return _dataService.Clusters.ToList().OrderBy(x => Guid.NewGuid().ToString()).First().Id;
        }
    }
}