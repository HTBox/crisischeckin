using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestClass]
    public class DisasterServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDataService()
        {
            DisasterService service = new DisasterService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssignToVolunteer_NullDisaster()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.AssignToVolunteer(null, new Person(), new DateTime(2014, 01, 01), new DateTime(2014, 02, 02));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssignToVolunteer_NullPerson()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.AssignToVolunteer(new Disaster(), null, new DateTime(2014, 01, 01), new DateTime(2014, 02, 02));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_BeginDateGreaterThanEndDate()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.AssignToVolunteer(new Disaster(), new Person(), new DateTime(2013, 6, 13), new DateTime(2013, 5, 10));
        }

        [TestMethod]
        public void AssignToVolunteer_Valid()
        {
            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>())).Returns(new Commitment() {
                Id = 1,
                PersonId = 5,
                DisasterId = 10,
                StartDate = new DateTime(2014, 01, 01),
                EndDate = new DateTime(2014, 01, 01)
            });
            DisasterService service = new DisasterService(moqDataService.Object);

            var actual = service.AssignToVolunteer(new Disaster() { Id = 10, Name = "A disaster" },
                new Person() { Id = 5, Email = "bob.jones@email.com" },
                new DateTime(2013, 01, 01), new DateTime(2013, 02, 01));
            
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(5, actual.PersonId);
            Assert.AreEqual(10, actual.DisasterId);
            Assert.AreEqual("1/1/2014", actual.StartDate.ToShortDateString());
        }
    }
}
