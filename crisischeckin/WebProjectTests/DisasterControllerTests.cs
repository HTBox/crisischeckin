using System;
using NUnit.Framework;
using Moq;
using Services.Interfaces;
using crisicheckinweb.Controllers;
using Models;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Services.Exceptions;

namespace WebProjectTests
{
    [TestFixture]
    public class DisasterControllerTests
    {
        private DisasterController _controllerUnderTest;

        private Mock<IDisaster> _disaster;

        [SetUp]
        public void SetUp()
        {
            _disaster = new Mock<IDisaster>();

            _controllerUnderTest = new DisasterController(_disaster.Object);
        }

        [Test]
        public void Assign_ValidDataAdd_ReturnsListView()
        {
            // Arrange

            // Act
            var viewModel = new DisasterViewModel { Id = -1, Name ="test", IsActive = false};
            var response = _controllerUnderTest.Create(viewModel);

            // Assert
            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.Equals("/Disaster/List"));
        }

        [Test]
        public void Assign_ValidDataUpdate_ReturnsListView()
        {
            // Arrange

            // Act
            var viewModel = new DisasterViewModel { Id = 0, Name = "updated", IsActive = true };
            var response = _controllerUnderTest.Create(viewModel);

            // Assert

            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.Equals("/Disaster/List"));
        }

        [Test]
        public void Assign_duplicateName_ReturnsCreateView()
        {
            // Arrange
            _disaster.Setup(x => x.Create(
                It.IsAny<Disaster>())).Throws(new DisasterAlreadyExistsException());

            // Act
            var viewModel = new DisasterViewModel { Id = -1, Name = "test", IsActive = true };
            var response = _controllerUnderTest.Create(viewModel);

            // Assert
            var view = response as ViewResult;
            Assert.AreEqual("Create", view.ViewName);
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }
    }
}
