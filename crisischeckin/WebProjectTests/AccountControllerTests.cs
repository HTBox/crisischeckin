using Moq;
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
        private AccountController _controllerUnderTest;

        private Mock<IVolunteerService> _volunteerService;
        private Mock<ICluster> _cluster;
        private Mock<IWebSecurityWrapper> _webSecurity;
        private Mock<IPrincipal> _principal;
        private Mock<HttpContextBase> _httpContext;

        [TestInitialize]
        public void Setup()
        {
            _principal = new Mock<IPrincipal>();
            _httpContext = new Mock<HttpContextBase>();
            _httpContext.Setup(x => x.User).Returns(_principal.Object);

            _volunteerService = new Mock<IVolunteerService>();
            _cluster = new Mock<ICluster>();
            _webSecurity = new Mock<IWebSecurityWrapper>();
            _webSecurity.SetupGet(x => x.CurrentUserId).Returns(42);

            var reqContext = new RequestContext(_httpContext.Object, new RouteData());

            _controllerUnderTest = new AccountController(_volunteerService.Object, _cluster.Object, _webSecurity.Object);
            _controllerUnderTest.ControllerContext = new ControllerContext(reqContext, _controllerUnderTest);
        }

        private RegisterModel CreateValidRegisterModel()
        {
            return new RegisterModel
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "1234",
                UserName = "user",
                Email = "user@email.com",
                Password = "p@ssw0rd",
                ConfirmPassword = "p@ssw0rd",
                Cluster = 42
            };
        }

        [TestMethod]
        public void Register_DuplicateEmailAddress_ReturnsRegisterView_With_ModelState_Error()
        {
            // Arrange
            _volunteerService.Setup(x => x.EmailAlreadyInUse("existing@email.com")).Returns(true);

            // Act
            var model = CreateValidRegisterModel();
            model.Email = "existing@email.com";
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Register(model);

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Email"));
        }

        [TestMethod]
        public void Register_TooSimplePassword_ReturnsRegisterView_With_ModelState_Error()
        {
            // Arrange
            _volunteerService.Setup(x => x.EmailAlreadyInUse(It.IsAny<string>())).Returns(false);

            // Act
            var model = CreateValidRegisterModel();
            model.Password = model.ConfirmPassword = model.UserName;
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Register(model);

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.ContainsKey("Password"));
        }

        [TestMethod]
        public void Register_ErrorDuringUserCreation_ReturnsRegisterView_With_ModelState_Error()
        {
            // Arrange
            _volunteerService.Setup(x => x.EmailAlreadyInUse(It.IsAny<string>())).Returns(false);
            _webSecurity.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()))
                .Throws(new UserCreationException("fake error"));

            // Act
            var model = CreateValidRegisterModel();
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Register(model);

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);

            _volunteerService.Verify(x => x.Register(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void Register_Successful_Creation_Redirects_To_Home()
        {
            // Arrange
            const int newUserId = 1234;

            _volunteerService.Setup(x => x.EmailAlreadyInUse(It.IsAny<string>())).Returns(false);
            _webSecurity.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(newUserId);

            // Act
            var model = CreateValidRegisterModel();
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Register(model);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            _volunteerService.Verify(x => x.Register(
                model.FirstName,
                model.LastName,
                model.Email,
                model.PhoneNumber,
                model.Cluster,
                newUserId));
        }


        [TestMethod]
        public void ChangeContactInfo_Assign_ValidData_Redirects_To_ContactInfoChanged_View()
        {
            // Arrange
            _volunteerService.Setup(x => x.FindByUserId(It.IsAny<int>())).Returns(new Person());

            // Act
            var model = new ChangeContactInfoViewModel { Email = "test@neverEverUsedDomain123141.com", PhoneNumber = "123456789" };

            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);

            var response = _controllerUnderTest.ChangeContactInfo(model);

            // Assert
            var view = response as RedirectToRouteResult;
            Assert.IsNotNull(view);

            var action = view.RouteValues["action"];
            Assert.AreEqual("ContactInfoChanged", action.ToString());
        }

        [TestMethod]
        public void ChangeContactInfo_DuplicateEmailAddress_ReturnsChangeContactInfoView_With_ModelState_Error()
        {
            // Arrange
            _volunteerService.Setup(x => x.UpdateDetails(It.IsAny<Person>())).Throws<PersonEmailAlreadyInUseException>();

            // Act
            var model = new ChangeContactInfoViewModel { Email = "test@UsedDomain123141.com", PhoneNumber = "123456789" };

            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);

            var response = _controllerUnderTest.ChangeContactInfo(model);

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
            // Arrange

            // Act
            var model = new ChangeContactInfoViewModel { Email = "test@UsedDomain123141.com", PhoneNumber = "" };

            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);

            var response = _controllerUnderTest.ChangeContactInfo(model);

            // Assert
            var result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("ChangeContactInfo"));
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);
        }
    }
}
