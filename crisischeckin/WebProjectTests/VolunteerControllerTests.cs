using crisicheckinweb.Controllers;
using crisicheckinweb.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProjectTests
{
    [TestClass]
    public class VolunteerControllerTests
    {
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
        }

        private VolunteerController CreateVolunteerController()
        {
            return new VolunteerController(_disasterSvc.Object, _clusterSvc.Object, _adminSvc.Object, _messageSvc.Object);
        }

        [TestMethod]
        public void Lookup_UnfilteredVolunteers_For_Disaster_Returns_AllVolunteers_ForTheDisaster()
        {
            //Arrange
            int disasterId = 2;
            var disaster = new Disaster();
            _disasterSvc.Setup(x => x.Get(disasterId)).Returns(disaster);
            var allVolunteers = new Collection<Person>();
            _adminSvc.Setup(x => x.GetVolunteers(disaster)).Returns(allVolunteers);

            var controller = CreateVolunteerController();
            //Act

            var response = controller.Filter(new ListByDisasterViewModel() { SelectedDisaster = disasterId, CommitmentDate = null });

            //Assert
            Assert.AreEqual(allVolunteers, response.Model);
        }

        [TestMethod]
        public void Lookup_FilteredVolunteers_For_Disaster_Returns_Volunteers_ForTheDisaster_FilteredByDate()
        {
            //Arrange
            int disasterId = 2;
            DateTime filteredDateTime = new DateTime(2014, 6, 3, 10, 8, 6);
            var disaster = new Disaster();
            _disasterSvc.Setup(x => x.Get(disasterId)).Returns(disaster);
            var filteredVolunteers = new Collection<Person>();
            _adminSvc.Setup(x => x.GetVolunteersForDate(disaster, filteredDateTime)).Returns(filteredVolunteers);

            var controller = CreateVolunteerController();
            //Act

            var response = controller.Filter(new ListByDisasterViewModel() { SelectedDisaster = disasterId, CommitmentDate = filteredDateTime });

            //Assert
            Assert.AreEqual(filteredVolunteers, response.Model);
        }
    }
}
