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

namespace WebProjectTests
{
    [TestClass]
    public class AccountControllerTests
    {
        //TODO: httpContext needs to be set
        [TestMethod]
        public void Assign_ValidData_ReturnsContactInfoChangedView()
        {
            /*
            // Arrange
            var volunteer = new Mock<IVolunteer>();
            var cluster = new Mock<ICluster>();
            var webSecurity = new Mock<IWebSecurityWrapper>();

            var controller = new AccountController(volunteer.Object, cluster.Object);

            volunteer.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            webSecurity.SetupGet(x => x.CurrentUserId).Returns(10);

            // Act
            var viewModel = new ChangeContactInfoViewModel { Email = "test@neverEverUsedDomain123141.com", PhoneNumber = "123456789" };
            var response = controller.ChangeContactInfo(viewModel);

            // Assert
            var view = response as ViewResult;
            Debug.WriteLine(view.ViewName);
            Assert.IsTrue(view.ViewName.Equals("ContactInfoChanged"));
        }

        [TestMethod]
        public void Assign_DuplicateEmailAddress_ReturnsChangeContactInfoView()
        {
            // Arrange
            var volunteer = new Mock<IVolunteer>();
            var cluster = new Mock<ICluster>();
            var webSecurity = new Mock<IWebSecurityWrapper>();

            var controller = new AccountController(volunteer.Object, cluster.Object);

            volunteer.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());

            var viewModel1 = new ChangeContactInfoViewModel { Email = "test@UsedDomain123141.com", PhoneNumber = "123456789" };
            var viewModel2 = new ChangeContactInfoViewModel { Email = "test@UsedDomain123141.com", PhoneNumber = "234567890" };

            // Act
            webSecurity.SetupGet(x => x.CurrentUserId).Returns(10);
            var response1 = controller.ChangeContactInfo(viewModel1);
            webSecurity.SetupGet(x => x.CurrentUserId).Returns(9);
            var response2 = controller.ChangeContactInfo(viewModel2);

            // Assert
            var view1 = response1 as ViewResult;
            var view2 = response2 as ViewResult;
            Assert.IsTrue(view1.ViewName.Equals("ContactInfoChanged"));

            Assert.IsTrue(view2.ViewName.Equals("ChangeContactInfo"));
            Assert.IsTrue(view2.ViewData.ModelState.Count >= 1);
        }
             */
    }
}
