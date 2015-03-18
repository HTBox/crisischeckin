using System;
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
using crisicheckinweb;
using Common;
using Services.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

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
        private Mock<IMessageService> _messageService;
        private RouteCollection _routeCollection;

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
            _messageService = new Mock<IMessageService>();

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/"));
            _httpContext.Setup(ctx => ctx.Request).Returns(request.Object);
            _httpContext.SetupGet(x => x.Request).Returns(request.Object);

            var response = new Mock<HttpResponseBase>();
            response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);
            _httpContext.Setup(ctx => ctx.Response).Returns(response.Object);
            _httpContext.SetupGet(x => x.Response).Returns(response.Object);

            var reqContext = new RequestContext(_httpContext.Object, new RouteData());

            _controllerUnderTest = new AccountController(_volunteerService.Object, _cluster.Object, _webSecurity.Object, _messageService.Object);
            _controllerUnderTest.ControllerContext = new ControllerContext(reqContext, _controllerUnderTest);

            _routeCollection = new RouteCollection();
            RouteConfig.RegisterRoutes(_routeCollection);
            _controllerUnderTest.Url = new UrlHelper(reqContext, _routeCollection);
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
            int userId;
            _webSecurity.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), out userId))
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
        public void Register_Successful_Creation_Redirects_To_RegistrationSuccessfulPage()
        {
            // Arrange
            int newUserId = 1234;
            const string confirmationToken = "t-o-k-e-n";
            var volunteer = new Person();

            _volunteerService.Setup(x => x.EmailAlreadyInUse(It.IsAny<string>()))
                .Returns(false);
            _volunteerService.Setup(x => x.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(volunteer);
            _webSecurity.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), out newUserId))
                .Returns(confirmationToken);

            // Act
            var model = CreateValidRegisterModel();
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Register(model);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("Account", result.RouteValues["controller"]);
            Assert.AreEqual("RegistrationSuccessful", result.RouteValues["action"]);

            _volunteerService.Verify(x => x.Register(
                model.FirstName,
                model.LastName,
                model.Email,
                model.PhoneNumber,
                model.Cluster,
                newUserId));

            _messageService.Verify(x => x.SendMessage(
                It.IsAny<Message>(),
                volunteer,
                It.IsAny<string>()));
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

        [TestMethod]
        public void ForgotPassword_ValidUserName_SendsEmail_And_RedirectsTo_PasswordResetRequestedView()
        {
            // Arrange
            const int existingUserId = 42;
            const string existingUser = "existing-user";
            const string token = "t-o-k-e-n";
            var person = new Person { Id = existingUserId };

            _webSecurity.Setup(x => x.GetUserId(existingUser))
                .Returns(existingUserId);
            _webSecurity.Setup(x => x.GeneratePasswordResetToken(existingUser))
                .Returns(token);
            _volunteerService.Setup(x => x.FindByUserId(existingUserId))
                .Returns(person);

            _routeCollection.MapRoute(
                name: "PasswordReset",
                url: "{controller}/{action}",
                defaults: new { controller = "Account", action = "PasswordReset" }
            );

            // Act
            var model = new ForgotPasswordViewModel
            {
                UserNameOrEmail = existingUser
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.ForgotPassword(model);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("PasswordResetRequested", result.RouteValues["action"]);

            _messageService.Verify(x => x.SendMessage(It.IsAny<Message>(), person, It.IsAny<string>()));
        }

        [TestMethod]
        public void ForgotPassword_ValidEmailInsteadOfUsername_SendsEmail_And_RedirectsTo_PasswordResetRequestedView()
        {
            // Arrange
            const string usernameOrEmail = "existing.email@test.com";
            const int existingUserId = 42;
            const string existingUsername = "testuser";
            const string token = "t-o-k-e-n";
            var person = new Person {Id = existingUserId};

            _webSecurity.Setup(x => x.GetUserId(usernameOrEmail))
                .Returns(-1);
            _volunteerService.Setup(x => x.FindUserByEmail(usernameOrEmail))
                .Returns(new User { Id = existingUserId, UserName = existingUsername});
            _webSecurity.Setup(x => x.GeneratePasswordResetToken(existingUsername))
                .Returns(token);
            _volunteerService.Setup(x => x.FindByUserId(existingUserId))
                .Returns(person);

            _routeCollection.MapRoute(
                name: "PasswordReset",
                url: "{controller}/{action}",
                defaults: new { controller = "Account", action = "PasswordReset" }
            );

            // Act
            var model = new ForgotPasswordViewModel
            {
                UserNameOrEmail = usernameOrEmail
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.ForgotPassword(model);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("PasswordResetRequested", result.RouteValues["action"]);

            _messageService.Verify(x => x.SendMessage(It.IsAny<Message>(), person, It.IsAny<string>()));
        }

        [TestMethod]
        public void ForgotPassword_InvalidUserName_DoesntSendEmail_But_RedirectsTo_PasswordResetRequestedView()
        {
            // Arrange
            const string nonExistingUser = "non-existing-user";

            _webSecurity.Setup(x => x.GetUserId(nonExistingUser)).Returns(-1);

            // Act
            var model = new ForgotPasswordViewModel
            {
                UserNameOrEmail = nonExistingUser
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.ForgotPassword(model);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("PasswordResetRequested", result.RouteValues["action"]);

            _messageService.Verify(x => x.SendMessage(It.IsAny<Message>(), It.IsAny<Person>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ResetPassword_Succesful_ResetsPasswords_And_RedirectsTo_PasswordResetCompletedView()
        {
            // Arrange
            const string token = "t-o-k-e-n";
            const string password = "p@ssw0rd";
            _webSecurity.Setup(x => x.ResetPassword(token, password)).Returns(true);

            // Act
            var model = new ResetPasswordViewModel
            {
                Token = token,
                NewPassword = password,
                ConfirmPassword = password
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.ResetPassword(model);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("PasswordResetCompleted", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ResetPassword_Failure_Returns_ResetPasswordView_With_ModelState_Error()
        {
            // Arrange
            const string invalidToken = "i-n-v-a-l-i-d";
            const string password = "p@ssw0rd";
            _webSecurity.Setup(x => x.ResetPassword(invalidToken, password)).Returns(false);

            // Act
            var model = new ResetPasswordViewModel
            {
                Token = invalidToken,
                NewPassword = password,
                ConfirmPassword = password
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.ResetPassword(model);

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);
        }

        [TestMethod]
        public void Login_ValidUsernameAndPassword_And_IsAdmin()
        {
            // Arrange
            const string validUserName = "administrator";
            const string validPassword = "p@ssw0rd";

            _webSecurity.Setup(x => x.Login(validUserName, validPassword, It.IsAny<bool>()))
                .Returns(true);
            _webSecurity.Setup(x => x.IsUserInRole(validUserName, Constants.RoleAdmin))
                .Returns(true);

            // Act
            var model = new LoginModel
            {
                UserNameOrEmail = validUserName,
                Password = validPassword
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Login(model, "/return/url");

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("Disaster", result.RouteValues["controller"]);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Login_ValidUsernameAndPassword_And_IsNoAdmin()
        {
            // Arrange
            const string validUserName = "volunteer";
            const string validPassword = "p@ssw0rd";

            _webSecurity.Setup(x => x.Login(validUserName, validPassword, It.IsAny<bool>()))
                .Returns(true);
            _webSecurity.Setup(x => x.IsUserInRole(validUserName, Constants.RoleAdmin))
                .Returns(false);

            // Act
            var model = new LoginModel
            {
                UserNameOrEmail = validUserName,
                Password = validPassword
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Login(model, "/return/url");

            // Assert
            var result = response as RedirectResult;
            Assert.AreEqual("/return/url", result.Url);
        }

        [TestMethod]
        public void Login_ValidEmailAndPassword_And_IsAdmin()
        {
            // Arrange
            const string validEmail = "administrator@email.com";
            const string validUserName = "administrator";
            const string validPassword = "p@ssw0rd";

            _webSecurity.Setup(x => x.Login(validEmail, validPassword, It.IsAny<bool>()))
                .Returns(false);
            _volunteerService.Setup(x => x.FindUserByEmail(validEmail))
                .Returns(new User { Id = 42, UserName = validUserName });
            _webSecurity.Setup(x => x.Login(validUserName, validPassword, It.IsAny<bool>()))
                .Returns(true);
            _webSecurity.Setup(x => x.IsUserInRole(validUserName, Constants.RoleAdmin))
                .Returns(true);

            // Act
            var model = new LoginModel
            {
                UserNameOrEmail = validUserName,
                Password = validPassword
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Login(model, "/return/url");

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("Disaster", result.RouteValues["controller"]);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Login_ValidEmailAndPassword_And_IsNoAdmin()
        {
            // Arrange
            const string validEmail = "administrator@email.com";
            const string validUserName = "administrator";
            const string validPassword = "p@ssw0rd";

            _webSecurity.Setup(x => x.Login(validEmail, validPassword, It.IsAny<bool>()))
                .Returns(false);
            _volunteerService.Setup(x => x.FindUserByEmail(validEmail))
                .Returns(new User { Id = 42, UserName = validUserName });
            _webSecurity.Setup(x => x.Login(validUserName, validPassword, It.IsAny<bool>()))
                .Returns(true);
            _webSecurity.Setup(x => x.IsUserInRole(validUserName, Constants.RoleAdmin))
                .Returns(false);

            // Act
            var model = new LoginModel
            {
                UserNameOrEmail = validUserName,
                Password = validPassword
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Login(model, "/return/url");

            // Assert
            var result = response as RedirectResult;
            Assert.AreEqual("/return/url", result.Url);
        }

        [TestMethod]
        public void Login_ValidEmailInvalidPassword_ReturnsLoginView_With_ModelState_Error()
        {
            // Arrange
            const string validUsernameOrEmail = "existing@email.com";
            const string existingUserName = "existing";
            const string invalidPassword = "invalidPass";

            _webSecurity.Setup(x => x.Login(validUsernameOrEmail, It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(false);
            _volunteerService.Setup(x => x.FindUserByEmail(validUsernameOrEmail))
                .Returns(new User { Id = 42, UserName = existingUserName });
            _webSecurity.Setup(x => x.Login(existingUserName, invalidPassword, It.IsAny<bool>()))
                .Returns(false);

            // Act
            var model = new LoginModel
            {
                UserNameOrEmail = validUsernameOrEmail,
                Password = invalidPassword
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Login(model, "/return/url");

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);
        }

        [TestMethod]
        public void Login_NonExistingUsernameAndEmail_ReturnsLoginView_With_ModelState_Error()
        {
            // Arrange
            const string invalidUsernameOrEmail = "non-existing";

            _webSecurity.Setup(x => x.Login(invalidUsernameOrEmail, It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(false);
            _volunteerService.Setup(x => x.FindUserByEmail(invalidUsernameOrEmail))
                .Returns((User)null);

            // Act
            var model = new LoginModel
            {
                UserNameOrEmail = invalidUsernameOrEmail,
                Password = "p@ssw0rd"
            };
            Mother.ControllerHelpers.SetupControllerModelState(model, _controllerUnderTest);
            var response = _controllerUnderTest.Login(model, "/return/url");

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count >= 1);
        }

        [TestMethod]
        public void ConfirmAccount_CorrectToken_Shows_SuccessfulMessage()
        {
            // Arrange
            const string token = "t-o-k-e-n";

            _webSecurity.Setup(x => x.ConfirmAccount(token))
                .Returns(true);

            // Act
            var response = _controllerUnderTest.ConfirmAccount(token);

            // Assert
            var result = response as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ConfirmAccount_InvalidToken_RedirectsTo_Home()
        {
            // Arrange
            const string token = "i-n-v-a-l-i-d";

            _webSecurity.Setup(x => x.ConfirmAccount(token))
                .Returns(false);

            // Act
            var response = _controllerUnderTest.ConfirmAccount(token);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ConfirmAccount_EmptyToken_RedirectsTo_Home()
        {
            // Arrange

            // Act
            var token = String.Empty;
            var response = _controllerUnderTest.ConfirmAccount(token);

            // Assert
            var result = response as RedirectToRouteResult;
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

    }
}
