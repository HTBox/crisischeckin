using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Interfaces;
using crisicheckinweb.Controllers;
using Models;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;
using System.Diagnostics;
using Services.Exceptions;

namespace WebProjectTests
{
    [TestClass]
    public class DisasterControllerTests
    {
        
        [TestMethod]
        public void Assign_ValidDataAdd_ReturnsListView()
        {
            
            // Arrange
            var disaster = new Mock<IDisaster>();

            var controller = new DisasterController(disaster.Object);

            // Act
            var viewModel = new Disaster { Id = -1, Name ="test", IsActive = false};
            var response = controller.Edit(viewModel);

            // Assert
            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.Equals("/Disaster/List"));
        }

        [TestMethod]
        public void Assign_ValidDataUpdate_ReturnsListView()
        {
            // Arrange
            var disaster = new Mock<IDisaster>();

            var controller = new DisasterController(disaster.Object);

            // Act
            var viewModel = new Disaster { Id = 0, Name = "updated", IsActive = true };
            var response = controller.Edit(viewModel);

            // Assert

            var result = response as RedirectResult;
            Assert.IsTrue(result.Url.Equals("/Disaster/List"));
        }

        [TestMethod]
        public void Assign_duplicateName_ReturnsEditView()
        {
            // Arrange
            var disaster = new Mock<IDisaster>();

            var controller = new DisasterController(disaster.Object);

            disaster.Setup(x => x.Create(
                It.IsAny<Disaster>())).Throws(new DisasterAlreadyExistsException());

            var viewModel = new Disaster { Id = -1, Name = "test", IsActive = true };
            var response = controller.Edit(viewModel);

            var view = response as ViewResult;
            Assert.IsTrue(view.ViewName.Equals("Edit"));
            Assert.IsTrue(view.ViewData.ModelState.Count >= 1);
        }
    }
}
