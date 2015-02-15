using Moq;
using Common;
using Models;
using crisicheckinweb.Controllers;
using crisicheckinweb.ViewModels;
using crisicheckinweb.Wrappers;
using Services.Interfaces;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Services.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebProjectTests
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IVolunteerService> volunteerService;
        private Mock<ICluster> cluster;
        private Mock<IWebSecurityWrapper> webSecurityWrapper;
        private Mock<IPrincipal> principal;
        private Mock<HttpContextBase> httpContext;
        private AccountController Controller;

        [TestInitialize]
        public void Setup()
        {
            principal = new Mock<IPrincipal>();
            httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(x => x.User).Returns(principal.Object);

            // Arrange
            volunteerService = new Mock<IVolunteerService>();
            cluster = new Mock<ICluster>();
            webSecurityWrapper = new Mock<IWebSecurityWrapper>();

            var reqContext = new RequestContext(httpContext.Object, new RouteData());

            Controller = new AccountController(volunteerService.Object, cluster.Object, webSecurityWrapper.Object);
            Controller.ControllerContext = new ControllerContext(reqContext, Controller);
        }

        

        [TestMethod]
        public void ChangeContactInfo_Assign_ValidData_Redirects_To_ContactInfoChanged_View()
        {
            volunteerService.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());
            webSecurityWrapper.SetupGet(x => x.CurrentUserId).Returns(10);

            // Act
            var model = new ChangeContactInfoViewModel { Email = "test@neverEverUsedDomain123141.com", PhoneNumber = "123456789" };

            Mother.ControllerHelpers.SetupControllerModelState(model, Controller);

            var response = Controller.ChangeContactInfo(model);

            // Assert
            var view = response as RedirectToRouteResult;
            Assert.IsNotNull(view);

            var action = view.RouteValues["action"];
            Assert.AreEqual("ContactInfoChanged", action.ToString());
        }

        [TestMethod]
        public void ChangeContactInfo_DuplicateEmailAddress_ReturnsChangeContactInfoView_With_ModelState_Error()
        {
            volunteerService.Setup(x => x.UpdateDetails(It.IsAny<Person>())).Throws<PersonEmailAlreadyInUseException>();

            var model = new ChangeContactInfoViewModel { Email = "test@UsedDomain123141.com", PhoneNumber = "123456789" };

            Mother.ControllerHelpers.SetupControllerModelState(model, Controller);
            
            // Act
            var response = Controller.ChangeContactInfo(model);
            
            // Assert
            var result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Email"));
            Assert.IsTrue(result.ViewName.Equals("ChangeContactInfo"));
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);
        }

        [TestMethod]
        public void ChangeContactInfo_Invalid_ModelState_Directs_User_To_ChangeContactInfo_View()
        {
            principal.Object.IsInRole(It.Is<string>(x => x == Constants.RoleAdmin));

            var model = new ChangeContactInfoViewModel { Email = "test@UsedDomain123141.com", PhoneNumber = "" };

            Mother.ControllerHelpers.SetupControllerModelState(model, Controller);
            
            // Act
            var response = Controller.ChangeContactInfo(model);

            // Assert
            var result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("ChangeContactInfo"));
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);
        }
    }
}
