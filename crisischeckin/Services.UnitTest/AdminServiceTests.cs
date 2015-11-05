using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestFixture]
    public class AdminServiceTests
    {
        private const int disasterWithVolunteersID = 1;
        private const int disasterWithNoVolunteersID = 2;

        private const int personWithCommitmentsID = 11;
        private const int personWithNoCommitmentsID = 12;
        private const int checkedInPersonID = 13;

        private const int commitmentId = 101;
        private const int checkedInCommitmentId = 102;

        private readonly Disaster disasterWithCommitments = new Disaster
        {
            Id = disasterWithVolunteersID,
            Name = "Post Conference party cleanup"
        };

        private readonly Disaster disasterWithNoCommitments = new Disaster
        {
            Id = disasterWithNoVolunteersID,
            Name = "Post Conference security deposit grovelling"
        };

        private readonly Person personWithCommitments = new Person
        {
            Id = personWithCommitmentsID,
            FirstName = "Richard",
            LastName = "Campbell",
            PhoneNumber = "(111) 555-1212",
            Email = "unused@nothere.com"
        };

        private readonly Person checkedInPerson = new Person
        {
            Id = checkedInPersonID,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "(111) 555-9999",
            Email = "john@nothere.com"
        };

        private readonly Person personWithNoCommitments = new Person
        {
            Id = personWithNoCommitmentsID,
            FirstName = "Lazy",
            LastName = "Volunteer",
            PhoneNumber = "(111) 555-1414",
            Email = "DontCallMe@nothere.com"
        };

        private readonly Commitment commitment = new Commitment
                            {
                                DisasterId = disasterWithVolunteersID,
                                Id = commitmentId,
                                PersonId = personWithCommitmentsID,
                                PersonIsCheckedIn = false,
                                StartDate = new DateTime(2013, 8, 10),
                                EndDate = new DateTime(2013, 8, 15)
                            };

        private readonly Commitment checkedInCommitment = new Commitment
                            {
                                DisasterId = disasterWithVolunteersID,
                                Id = checkedInCommitmentId,
                                PersonId = checkedInPersonID,
                                PersonIsCheckedIn = true,
                                StartDate = new DateTime(2013, 8, 10),
                                EndDate = new DateTime(2013, 8, 15)
                            };

        private Mock<IDataService> mockService;

        [SetUp]
        public void CreateDependencies()
        {
            mockService = new Mock<IDataService>();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenServiceIsNullConstructorThrowsExceptions()
        {
            var underTest = new AdminService(default(IDataService));
        }

        [Test]
        public void WhenOneVolunteerHasRegisteredReturnThatOneRecord()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }

        //with empty collections, return an empty list.
        [Test]
        public void WhenNoVolunteersAreRegisteredReturnAnEmptyList()
        {
            var underTest = new AdminService(mockService.Object);

            initializeDisasterCollection(disasterWithNoCommitments);

            var result = underTest.GetVolunteers(disasterWithNoCommitments);
            Assert.IsFalse(result.Any());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenDisasterIsNullThrowNullArgumentException()
        {
            var underTest = new AdminService(mockService.Object);
            var result = underTest.GetVolunteers(default(Disaster));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenDisasterIsNotFoundThrowArgumentException()
        {
            var underTst = new AdminService(mockService.Object);

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var disaster = new Disaster
            {
                Id = -1,
                Name = "DoesNotExist"
            };
            underTst.GetVolunteers(disaster);
        }

        [Test]
        public void WhenVolunteerDontCommitReturnCommittedRecords()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithNoCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }


        // person with multiple commitments only once
        [Test]
        public void WhenOneVolunteerHasMultipleRegistrationsReturnExactlyOneRecord()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);

            var secondCommitment = new Commitment
            {
                DisasterId = disasterWithVolunteersID,
                Id = 102,
                PersonId = personWithNoCommitmentsID,
                StartDate = new DateTime(2013, 8, 15),
                EndDate = new DateTime(2013, 8, 20)
            };
            initializeCommitmentCollection(commitment, secondCommitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }

        // people with commitments on other disasters does not show up
        // person with multiple commitments only once
        [Test]
        public void WhenVolunteersExistForMultipleDisastersReturnCorrectDisaster()
        {
            initializeDisasterCollection(disasterWithCommitments, disasterWithNoCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithNoCommitments);

            var secondCommitment = new Commitment
            {
                DisasterId = disasterWithNoVolunteersID,
                Id = 102,
                PersonId = personWithNoCommitmentsID,
                StartDate = new DateTime(2013, 8, 15),
                EndDate = new DateTime(2013, 8, 20)
            };
            initializeCommitmentCollection(commitment, secondCommitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetVolunteersFiltersCorrectlyWhenRequestingCheckedInOnly()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, checkedInPerson);
            initializeCommitmentCollection(commitment, checkedInCommitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments, checkedInOnly:true);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void WhenOneVolunteerHasRegisteredReturnOneRecordInRange()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013, 08, 12), clusterCoordinatorsOnly: false);

            Assert.AreEqual(1, result.Count());
        }


        [Test]
        public void WhenOneVolunteerHasRegisteredButIsNotCheckedInReturnNoRecord()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013, 08, 12), clusterCoordinatorsOnly: false, checkedInOnly: true);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetVolunteersByDate_FiltersToOnlyClusterCoordinators()
        {
            var otherCommitment = new Commitment
            {
                DisasterId = disasterWithCommitments.Id,
                PersonId = personWithNoCommitmentsID,
                Id = 102,
                StartDate = new DateTime(2013, 8, 10),
                EndDate = new DateTime(2013, 8, 15)
            };

            var clusterCoordinator = new ClusterCoordinator
            {
                Id = 1001,
                DisasterId = disasterWithCommitments.Id,
                PersonId = personWithCommitmentsID,
            };

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithNoCommitments);
            initializeCommitmentCollection(commitment, otherCommitment);
            mockService.Setup(ds => ds.ClusterCoordinators).Returns(new[] { clusterCoordinator }.AsQueryable());

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013, 08, 12), clusterCoordinatorsOnly: true);

            Assert.AreEqual(1, result.Count());
        }


        [Test]
        public void GetVolunteersByDate_FiltersToOnlyCheckedInClusterCoordinators()
        {
            var checkedInNonCoordinatorCommitment = new Commitment
            {
                DisasterId = disasterWithCommitments.Id,
                PersonId = personWithNoCommitmentsID,
                Id = 102,
                StartDate = new DateTime(2013, 8, 10),
                EndDate = new DateTime(2013, 8, 15)
            };

            var clusterCoordinator = new ClusterCoordinator
            {
                Id = 1001,
                DisasterId = disasterWithCommitments.Id,
                PersonId = checkedInPersonID,
            };

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, checkedInPerson, personWithNoCommitments);
            initializeCommitmentCollection(commitment, checkedInCommitment, checkedInNonCoordinatorCommitment);
            mockService.Setup(ds => ds.ClusterCoordinators).Returns(new[] { clusterCoordinator }.AsQueryable());

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013, 08, 12), clusterCoordinatorsOnly: true, checkedInOnly: true);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void WhenOneVolunteerHasRegisteredReturnNoRecordsInRange()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013, 08, 5), clusterCoordinatorsOnly: false);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetVolunteersForDate_DoesNotReturnDuplicatePeople_WhenCommitmentsOverlap()
        {
            var mockData = new Mock<IDataService>();
            var commitments = new List<Commitment>();
            commitments.Add(new Commitment { DisasterId = 42, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today.AddDays(1), PersonId = 1 });
            commitments.Add(new Commitment { DisasterId = 42, StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(2), PersonId = 1 });
            mockData.Setup(x => x.Commitments).Returns(commitments.AsQueryable());
            var disasters = new List<Disaster>();
            disasters.Add(new Disaster { Id = 42, Name = "disaster", IsActive = true });
            mockData.Setup(x => x.Disasters).Returns(disasters.AsQueryable());
            var persons = new List<Person>();
            persons.Add(new Person { Id = 1});
            mockData.Setup(x => x.Persons).Returns(persons.AsQueryable());

            var underTest = new AdminService(mockData.Object);

            var actual = underTest.GetVolunteersForDate(42, DateTime.Today, clusterCoordinatorsOnly: false);

            Assert.AreEqual(1, actual.Count());
        }

        [Test]
        public void GetVolunteersForDisasterFilteredByDateReturnsExpectedRecord()
        {
            var personWithDifferentCommitmentDateRange = personWithNoCommitments;
            const int personWithDifferentCommitmentDateRangeId = personWithNoCommitmentsID;

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithDifferentCommitmentDateRange);

            var secondCommitmentOutOfDateRange = new Commitment
            {
                DisasterId = disasterWithVolunteersID,
                Id = 102,
                PersonId = personWithDifferentCommitmentDateRangeId,
                StartDate = new DateTime(2013, 9, 15),
                EndDate = new DateTime(2013, 9, 20)
            };
            initializeCommitmentCollection(commitment, secondCommitmentOutOfDateRange);

            var systemUnderTest = new AdminService(mockService.Object);

            var result = systemUnderTest.GetVolunteersForDisaster(disasterWithCommitments.Id, new DateTime(2013, 8, 12));

            var actual = result as IList<Person> ?? result.ToList();
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(actual.First().Id, personWithCommitmentsID);
        }

        [Test]
        public void GetVolunteersForDisasterUnfilteredByDateReturnsExpectedRecords()
        {
            var personWithDifferentCommitmentDateRange = personWithNoCommitments;
            const int personWithDifferentCommitmentDateRangeId = personWithNoCommitmentsID;

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithDifferentCommitmentDateRange);

            var secondCommitment = new Commitment
            {
                DisasterId = disasterWithVolunteersID,
                Id = 102,
                PersonId = personWithDifferentCommitmentDateRangeId,
                StartDate = new DateTime(2013, 9, 15),
                EndDate = new DateTime(2013, 9, 20)
            };
            initializeCommitmentCollection(commitment, secondCommitment);

            var systemUnderTest = new AdminService(mockService.Object);

            var result = systemUnderTest.GetVolunteersForDisaster(disasterWithCommitments.Id, null);

            var actual = result as IList<Person> ?? result.ToList();
            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.FirstOrDefault(p => p.Id == personWithCommitments.Id) != null, 
                "Could not find first expected Person.");
            Assert.IsTrue(actual.FirstOrDefault(p => p.Id == personWithDifferentCommitmentDateRange.Id) != null,
                "Could not find second expected Person.");
        }

        private void initializeDisasterCollection(params Disaster[] disasters)
        {
            mockService.Setup(ds => ds.Disasters).Returns(disasters.AsQueryable());
        }

        private void initializeVolunteerCollection(params Person[] people)
        {
            mockService.Setup(ds => ds.Persons).Returns(people.AsQueryable());
        }

        private void initializeCommitmentCollection(params Commitment[] commitments)
        {
            mockService.Setup(ds => ds.Commitments).Returns(commitments.AsQueryable());
        }
    }
}
