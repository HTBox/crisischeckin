using crisicheckinweb.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using System.Linq;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    [TestClass]
    public class Can_access_person : With_an_empty_database_environment
    {
        DataAccessHelper _dataAccessHelper;
        DataService _dataService;
        Person _person;

        [TestInitialize]
        public void Arrange()
        {
            _dataService = new DataService(new CrisisCheckin(), new CrisisCheckinMembership());
            _dataAccessHelper = new DataAccessHelper(_dataService);
            _person = _dataAccessHelper.Create_a_volunteer();
        }
    }
}
