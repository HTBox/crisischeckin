using System;
using Models;
using NUnit.Framework;
using Services;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    [TestFixture]
    public class Can_access_person : With_an_empty_database_environment
    {
        DataAccessHelper _dataAccessHelper;
        DataService _dataService;

        [Test]
        public void Arrange()
        {
            // Arrange
            _dataService = new DataService(new CrisisCheckin(), new CrisisCheckinMembership());
            _dataAccessHelper = new DataAccessHelper(_dataService);
            var volunteerService = new VolunteerService(_dataService);
            var previouslyCreatedPerson = _dataAccessHelper.Create_a_volunteer();

            // Act
            var volunteer = volunteerService.FindByUserId(previouslyCreatedPerson.UserId.Value);

            // Assert
            Assert.IsNotNull(volunteer);
            Assert.AreEqual(previouslyCreatedPerson.FirstName, volunteer.FirstName);
        }
    }
}
