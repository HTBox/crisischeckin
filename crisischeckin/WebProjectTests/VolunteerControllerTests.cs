using System.Collections.Generic;
using System.Linq;
using crisicheckinweb.Controllers;
using crisicheckinweb.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;
using System;

namespace WebProjectTests
{
    [TestClass]
    public class VolunteerControllerTests
    {
        private VolunteerController _controllerUnderTest;

        private Mock<IDisaster> _disasterSvc;
        private Mock<ICluster> _clusterSvc;
        private Mock<IAdmin> _adminSvc;
        private Mock<IMessageService> _messageSvc;

        [TestInitialize]
        public void TestInit()
        {
            _disasterSvc = new Mock<IDisaster>();
            _clusterSvc = new Mock<ICluster>();
            _adminSvc = new Mock<IAdmin>();
            _messageSvc = new Mock<IMessageService>();

            _controllerUnderTest = new VolunteerController(_disasterSvc.Object, _clusterSvc.Object, _adminSvc.Object, _messageSvc.Object);
        }

        [TestMethod]
        public void Lookup_UnfilteredVolunteers_For_Disaster_Returns_AllVolunteers_ForTheDisaster()
        {
            //Arrange
            int disasterId = 2;
            var disaster = new Disaster();
            _disasterSvc.Setup(x => x.Get(disasterId)).Returns(disaster);
            var allVolunteers = new List<Person>();
            _adminSvc.Setup(x => x.GetVolunteersForDisaster(disaster.Id, null, false)).Returns(allVolunteers);

            //Act
            var response = _controllerUnderTest.Filter(new ListByDisasterViewModel
            {
                SelectedDisaster = disasterId, CommitmentDate = null
            });

            //Assert
            var model = response.Model as IEnumerable<Person>;
            Assert.IsNotNull(model, "View Model is not an IEnumerable<Person>.");

            CollectionAssert.AreEquivalent(allVolunteers.ToArray(), model.ToArray());
        }

        [TestMethod]
        public void Lookup_FilteredVolunteers_For_Disaster_Returns_Volunteers_ForTheDisaster_FilteredByDate()
        {
            //Arrange
            int disasterId = 2;
            DateTime filteredDateTime = new DateTime(2014, 6, 3, 10, 8, 6);
            var disaster = new Disaster();
            _disasterSvc.Setup(x => x.Get(disasterId)).Returns(disaster);
            var filteredVolunteers = new List<Person>();
            _adminSvc.Setup(x => x.GetVolunteersForDisaster(disaster.Id, filteredDateTime, false)).Returns(filteredVolunteers);

            //Act
            var response = _controllerUnderTest.Filter(new ListByDisasterViewModel
            {
                SelectedDisaster = disasterId, CommitmentDate = filteredDateTime
            });

            //Assert
            var model = response.Model as IEnumerable<Person>;
            Assert.IsNotNull(model, "View Model is not an IEnumerable<Person>.");

            CollectionAssert.AreEquivalent(filteredVolunteers.ToArray(), model.ToArray());
        }
    }
}
