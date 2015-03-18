using System;
using System.Collections.Generic; 
using System.Linq;
using NUnit.Framework;
using Moq;
using Services.Interfaces;
using crisicheckinweb.Controllers;
using Models;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;

namespace WebProjectTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controllerUnderTest;

        private Mock<IDisaster> _disaster;
        private Mock<IVolunteerService> _volunteerService;
        private Mock<IWebSecurityWrapper> _webSecurity;
        private Mock<IClusterCoordinatorService> _clusterCoordinatorService;
        private Mock<IVolunteerTypeService> _volunteerTypeService;

        [SetUp]
        public void SetUp()
        {
            _disaster = new Mock<IDisaster>();
            _volunteerService = new Mock<IVolunteerService>();
            _webSecurity = new Mock<IWebSecurityWrapper>();
            _clusterCoordinatorService = new Mock<IClusterCoordinatorService>();
            _volunteerTypeService = new Mock<IVolunteerTypeService>();

            _controllerUnderTest = new HomeController(_disaster.Object, _volunteerService.Object, _webSecurity.Object, _clusterCoordinatorService.Object, _volunteerTypeService.Object);
        }

        [Test]
        public void Assign_BadStartDate_ReturnsIndexView()
        {
            // Arrange
            _volunteerService.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            _disaster.Setup(x => x.AssignToVolunteer(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>())).Throws(new ArgumentException(""));

            // Act
            var viewModel = new VolunteerViewModel { SelectedStartDate = DateTime.Today.AddDays(-1) };
            var response = _controllerUnderTest.Assign(viewModel);

            // Assert
            var view = response as ViewResult;
            Assert.IsTrue(view.ViewName.Equals("Index"));
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }

        [Test]
        public void Assign_BadDateRange_ReturnsIndexView()
        {
            // Arrange
            _volunteerService.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            _disaster.Setup(x => x.AssignToVolunteer(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>())).Throws(new ArgumentException(""));

            _webSecurity.SetupGet(x => x.CurrentUserId).Returns(10);

            // Act
            var viewModel = new VolunteerViewModel { SelectedStartDate = DateTime.Today.AddDays(1) };
            var response = _controllerUnderTest.Assign(viewModel);

            // Assert
            var view = response as ViewResult;
            Assert.IsTrue(view.ViewName.Equals("Index"));
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }

        [Test]
        public void Assign_ValidDateRange_RedirectsToHome()
        {
            // Arrange
            _volunteerService.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());

            // Act
            var viewModel = new VolunteerViewModel { SelectedStartDate = DateTime.Today.AddDays(1) };
            var response = _controllerUnderTest.Assign(viewModel);

            // Assert
            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.ToLower().Contains("home"));
        }

        [Test]
        public void RemoveCommitmentById_NotYourCommitment_ReturnsIndexView()
        {
            // Arrange
            _volunteerService.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());

            // Act
            var viewModel = new VolunteerViewModel { RemoveCommitmentId = int.MinValue };
            var response = _controllerUnderTest.RemoveCommitment(viewModel);

            // Assert
            var view = response as ViewResult;
            Assert.IsTrue(view.ViewName.Equals("Index"));
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }

        [Test]
        public void RemoveCommitmentById_Valid_RedirectsToHome()
        {
            // Arrange
            var commitments = new List<Commitment>() { new Commitment() { Id = 7, PersonId = 13}};

            _volunteerService.Setup(service => service.FindByUserId(It.IsAny<int>())).Returns(new Person() { Id = 13 });
            _volunteerService.Setup(service => service.RetrieveCommitments(13, true)).Returns(commitments.AsQueryable());

            // Act
            var viewModel = new VolunteerViewModel { RemoveCommitmentId = 7 };
            var response = _controllerUnderTest.RemoveCommitment(viewModel);

            // Assert
            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.ToLower().Contains("home"));
        }
    }
}
