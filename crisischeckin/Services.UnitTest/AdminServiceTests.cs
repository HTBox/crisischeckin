using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;
using Moq;
using Models;
using System.Collections.Generic;

namespace WebProjectTests.ServiceTests
{
    [TestClass]
    public class AdminServiceTests
    {
        private Mock<IDataService> mockService;

        [TestInitialize]
        public void CreateDependencies()
        {
            mockService = new Mock<IDataService>();
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void WhenServiceIsNullConstructorThrowsExceptions()
        {
            var underTest = new AdminService(default(IDataService));
        }

        [TestMethod]
        public void WhenOneVolunteerHasRegisteredReturnThatOneRecord()
        {
            const int disasterId = 1;
            const int personId = 2;
            const int commitmentId = 3;
            var disaster = new Disaster
                    {
                        Id = disasterId,
                        Name = "Post Conference party cleanup"
                    };
            mockService.Setup(ds => ds.Disasters).Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            mockService.Setup(ds => ds.Persons).Returns(new List<Person>
                {
                    new Person
                    {
                        Id=personId,
                        FirstName="Richard",
                        LastName="Campbell",
                        PhoneNumber="(111) 555-1212",
                        Email="unused@nothere.com"
                    }
                }.AsQueryable());
            mockService.Setup(ds => ds.Commitments).Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=disasterId,
                        Id=commitmentId,
                        PersonId=personId,
                        StartDate=new DateTime(2013, 8, 10),
                        EndDate = new DateTime(2013, 8, 15)
                    }
                }.AsQueryable());

            var underTest = new AdminService(mockService.Object);
            var result = underTest.GetVolunteers(disaster);
            Assert.AreEqual(1, result.Count());
        }

        //With no volunteers, return an empty list.
        [TestMethod]
        public void WhenNoVolunteersAreRegisteredReturnAnEmptyList()
        {
            var underTest = new AdminService(mockService.Object);

            var disaster = new Disaster();

            var result = underTest.GetVolunteers(disaster);
            Assert.IsFalse(result.Any());
        }



    }
}
