using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Interfaces;
using crisicheckinweb.Controllers;
using Models;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;

namespace WebProjectTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Assign_BadStartDate_ReturnsIndexView()
        {
            // Arrange
            var disaster = new Mock<IDisaster>();
            var volunteer = new Mock<IVolunteer>();
            var webSecurity = new Mock<IWebSecurityWrapper>();

            var controller = new HomeController(disaster.Object, volunteer.Object, webSecurity.Object);

            volunteer.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            webSecurity.SetupGet(x => x.CurrentUserId).Returns(10);

            // Act
            var viewModel = new VolunteerViewModel { SelectedStartDate = DateTime.Today.AddDays(-1) };
            var response = controller.Assign(viewModel);

            // Assert
            var view = response as ViewResult;
            Assert.IsTrue(view.ViewName.Equals("Index"));
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Assign_BadDateRange_ReturnsIndexView()
        {
            // Arrange
            var disaster = new Mock<IDisaster>();
            var volunteer = new Mock<IVolunteer>();
            var webSecurity = new Mock<IWebSecurityWrapper>();

            var controller = new HomeController(disaster.Object, volunteer.Object, webSecurity.Object);

            disaster.Setup(x => x.AssignToVolunteer(
                It.IsAny<Disaster>(),
                It.IsAny<Person>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>())).Throws(new ArgumentException(""));

            volunteer.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            webSecurity.SetupGet(x => x.CurrentUserId).Returns(10);

            // Act
            var viewModel = new VolunteerViewModel { SelectedStartDate = DateTime.Today.AddDays(1) };
            var response = controller.Assign(viewModel);

            // Assert
            var view = response as ViewResult;
            Assert.IsTrue(view.ViewName.Equals("Index"));
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }

        [TestMethod]
        public void Assign_ValidDateRange_RedirectsToHome()
        {
            // Arrange
            var disaster = new Mock<IDisaster>();
            var volunteer = new Mock<IVolunteer>();
            var webSecurity = new Mock<IWebSecurityWrapper>();
            
            var controller = new HomeController(disaster.Object, volunteer.Object, webSecurity.Object);

            // Act
            var viewModel = new VolunteerViewModel { SelectedStartDate = DateTime.Today.AddDays(1) };
            var response = controller.Assign(viewModel);

            // Assert
            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.ToLower().Contains("home"));
        }
    }
}
