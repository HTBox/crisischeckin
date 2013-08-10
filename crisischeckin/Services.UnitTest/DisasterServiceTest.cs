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
            moqDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>())).Returns(new Commitment()
            {
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

        [TestMethod]
        public void CreateDisaster_Valid()
        {
            // arrange
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster() { Name = "name", IsActive = true };
            moqDataService.Setup(m => m.AddDisaster(disaster)).Returns(disaster);
            DisasterService service = new DisasterService(moqDataService.Object);

            // act
            var result = service.Create(disaster);

            // assert
            Assert.AreEqual(disaster.Name, result.Name);
            Assert.AreEqual(disaster.IsActive, result.IsActive);
            moqDataService.Verify(m => m.AddDisaster(disaster));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNull()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.Create(null);
        }

         [TestMethod]
         [ExpectedException(typeof (ArgumentNullException))]
         public void CreateDisaster_DisasterNameNull()
         {
             var moqDataService = new Mock<IDataService>();
             DisasterService service = new DisasterService(moqDataService.Object);

             service.Create(new Disaster(){IsActive = true, Name=""});
         }
    }
}
