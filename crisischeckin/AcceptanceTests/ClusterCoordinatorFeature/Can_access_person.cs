using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using crisicheckinweb.Api;
using Services;

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
            _dataService = new DataService(new CrisisCheckin());
            _dataAccessHelper = new DataAccessHelper(_dataService);
            _person = _dataAccessHelper.Create_a_volunteer();
        }

        [TestMethod]
        public void Get_person()
        {
            var db = new CrisisCheckin();

            var controller = new BreezeController(db);
            Assert.AreEqual(_person.PhoneNumber, controller.Person().PhoneNumber);
        }
    }
}
