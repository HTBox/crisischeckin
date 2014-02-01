using System;
using System.Security.Principal;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Exceptions;
using Services.Interfaces;
using crisicheckinweb.Controllers;
using Models;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;

namespace WebProjectTests
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void Assign_ValidData_ReturnsContactInfoChangedView()
        {
            // Arrange
            var volunteer = new Mock<IVolunteerService>();
            var cluster = new Mock<ICluster>();
            var webSecurity = new Mock<ISecurity>();
            var principal = new GenericPrincipal(new GenericIdentity("johndoe"), new[] { Constants.RoleVolunteer });

            var controller = new AccountController(volunteer.Object, cluster.Object, webSecurity.Object, principal);

            volunteer.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            webSecurity.Setup(x => x.GetInfoForCurrentUser()).Returns(new UserInfo { Id = 10, Username = "johndoe" });

            // Act
            var viewModel = new ChangeContactInfoViewModel { Email = "test@neverEverUsedDomain123141.com", PhoneNumber = "123456789" };
            var response = (RedirectToRouteResult)controller.ChangeContactInfo(viewModel);

            // Assert
            Assert.IsTrue(response.RouteValues["action"].Equals("ContactInfoChanged"));
        }

        [TestMethod]
        public void Assign_DuplicateEmailAddress_ReturnsChangeContactInfoView()
        {
            // Arrange
            var volunteer = new Mock<IVolunteerService>();
            var cluster = new Mock<ICluster>();
            var webSecurity = new Mock<ISecurity>();
            var principal = new GenericPrincipal(new GenericIdentity("johndoe"), new[] { Constants.RoleVolunteer });

            var controller = new AccountController(volunteer.Object, cluster.Object, webSecurity.Object, principal);

            volunteer.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            volunteer.Setup(x => x.UpdateDetails(It.IsAny<Person>())).Throws<PersonEmailAlreadyInUseException>();

            var viewModel = new ChangeContactInfoViewModel { Email = "test@neverEverUsedDomain123141.com", PhoneNumber = "123456789" };

            // Act
            webSecurity.Setup(x => x.GetInfoForCurrentUser()).Returns(new UserInfo { Id = 10, Username = "johndoe" });
            var response = (ViewResult)controller.ChangeContactInfo(viewModel);

            // Assert
            Assert.IsTrue(response.ViewName.Equals(String.Empty));
            Assert.IsTrue(response.ViewData.ModelState.Count >= 1);
        }
    }
}
