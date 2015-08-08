using System;
using System.Web.Mvc;
using System.Web.Routing;
using crisicheckinweb.Filters;
using crisicheckinweb.Infrastructure;
using Common;
using crisicheckinweb.ViewModels;
using Models;
using Services.Exceptions;
using Services.Interfaces;
using crisicheckinweb.Wrappers;
using Resources;
using Services;

namespace crisicheckinweb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IVolunteerService _volunteerSvc;
        private readonly ICluster _clusterSvc;
        private readonly IWebSecurityWrapper _webSecurity;
        private readonly IMessageService _messageService;

        public AccountController(IVolunteerService volunteerSvc, ICluster clusterSvc, IWebSecurityWrapper webSecurity, IMessageService messageService)
        {
            _clusterSvc = clusterSvc;
            _webSecurity = webSecurity;
            _volunteerSvc = volunteerSvc;
            _messageService = messageService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // First assume the username was typed in.
                    if (_webSecurity.Login(model.UserNameOrEmail, model.Password, model.RememberMe))
                    {
                        if (_webSecurity.IsUserInRole(model.UserNameOrEmail, Constants.RoleAdmin))
                        {
                            return RedirectToAction("List", "Disaster");
                        }
                        return RedirectToLocal(returnUrl);
                    }

                    // If login fails, assume the email was typed in instead.
                    var user = _volunteerSvc.FindUserByEmail(model.UserNameOrEmail);
                    if (user != null)
                    {
                        if (_webSecurity.Login(user.UserName, model.Password, model.RememberMe))
                        {
                            if (_webSecurity.IsUserInRole(user.UserName, Constants.RoleAdmin))
                            {
                                return RedirectToAction("List", "Disaster");
                            }
                            return RedirectToLocal(returnUrl);
                        }
                    }
                }
                catch (UserNotActivatedException)
                {
                    ModelState.AddModelError("", "Your account has to be confirmed by the link sent in the email before you can login.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The username/email or password provided is incorrect.");
            return View(model);
        }


        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            _webSecurity.Logout();

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    if (_volunteerSvc.EmailAlreadyInUse(model.Email))
                        throw new PersonAlreadyExistsException();

                    string errorMessage;
                    if (PasswordComplexity.IsValid(model.Password, model.UserName, out errorMessage))
                    {
                        int userId;
                        string token = _webSecurity.CreateUser(model.UserName, model.Password, new[] { Constants.RoleVolunteer }, out userId);
                        var volunteer = _volunteerSvc.Register(model.FirstName, model.LastName, model.Email, model.PhoneNumber, userId);
                        if (volunteer != null)
                        {
                            // Generate the absolute Url for the account activation action.
                            var routeValues = new RouteValueDictionary { { "token", token } };
                            var accountActivationLink = Url.Action("ConfirmAccount", "Account", routeValues, Request.Url.Scheme);

                            var body = String.Format(@"<p>Click on the following link to activate your account: <a href='{0}'>{0}</a></p>", accountActivationLink);
                            var message = new Message("CrisisCheckin - Activate your account", body);

                            _messageService.SendMessage(message, volunteer);
                        }

                        return RedirectToAction("RegistrationSuccessful", "Account");
                    }
                    ModelState.AddModelError("Password", errorMessage ?? DefaultErrorMessages.InvalidPasswordFormat);
                }
                catch (PersonAlreadyExistsException)
                {
                    ModelState.AddModelError("Email", "Email is already in use!");
                }
                catch (UserCreationException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegistrationSuccessful()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmAccount(string token)
        {
            if (!String.IsNullOrWhiteSpace(token))
            {
                if (_webSecurity.ConfirmAccount(token))
                {
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // First assume the username was typed in.
                var userName = model.UserNameOrEmail;
                var userId = _webSecurity.GetUserId(model.UserNameOrEmail);
                if (userId == -1)
                {
                    // If the user was not found by name, assume his email was typed in.
                    var user = _volunteerSvc.FindUserByEmail(model.UserNameOrEmail);
                    if (user != null)
                    {
                        userName = user.UserName;
                        userId = user.Id;
                    }
                }

                // Only send email when user actually exists. For security reasons
                // don't show an error when the given user doesn't exist.
                if (userId != -1)
                {
                    var volunteer = _volunteerSvc.FindByUserId(userId);
                    if (volunteer != null)
                    {
                        var token = _webSecurity.GeneratePasswordResetToken(userName);
                        // Generate the absolute Url for the password reset action.
                        var routeValues = new RouteValueDictionary { { "token", token } };
                        var passwordResetLink = Url.Action("ResetPassword", "Account", routeValues, Request.Url.Scheme);

                        var body = String.Format(@"<p>Click on the following link to reset your password: <a href='{0}'>{0}</a></p>", passwordResetLink);
                        var message = new Message("CrisisCheckin - Password Reset", body);

                        _messageService.SendMessage(message, volunteer);
                    }
                }
                return RedirectToAction("PasswordResetRequested");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordResetRequested()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            var model = new ResetPasswordViewModel
            {
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_webSecurity.ResetPassword(model.Token, model.NewPassword))
                {
                    return RedirectToAction("PasswordResetCompleted");
                }
                ModelState.AddModelError("", "An error occured while resetting your password.");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordResetCompleted()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public string CheckPasswordValidity(string userName, string password)
        {
            string errorMessage;
            if (String.IsNullOrWhiteSpace(userName))
            {
                userName = _webSecurity.CurrentUserName;
            }
            PasswordComplexity.IsValid(password, userName, out errorMessage);
            return errorMessage;
        }


        public ActionResult ChangePassword()
        {
            return View("ChangePassword", DetermineLayout(), null);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_webSecurity.ValidateUser(_webSecurity.CurrentUserName, model.OldPassword))
                {
                    string errorMessage;
                    if (PasswordComplexity.IsValid(model.NewPassword, _webSecurity.CurrentUserName, out errorMessage))
                    {
                        _webSecurity.ChangePassword(_webSecurity.CurrentUserName, model.OldPassword, model.NewPassword);
                        return RedirectToAction("PasswordChanged");
                    }
                    ModelState.AddModelError("NewPassword", errorMessage ?? DefaultErrorMessages.InvalidPasswordFormat);
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Old password is not correct.");
                }
            }
            return View("ChangePassword", DetermineLayout(), null);
        }

        public ActionResult PasswordChanged()
        {
            return View();
        }

        public ActionResult ChangeContactInfo()
        {
            if (_webSecurity.CurrentUserId == 1)
            {
                TempData["AdminContactError"] = "Administrator is not allowed to have contact details!";
                return RedirectToAction("Index", "Home");
            }
            var personToUpdate = _volunteerSvc.GetPersonDetailsForChangeContactInfo(_webSecurity.CurrentUserId);
            if (personToUpdate != null)
            {
                ChangeContactInfoViewModel model = new ChangeContactInfoViewModel { Email = personToUpdate.Email, PhoneNumber = personToUpdate.PhoneNumber };
                return View(model);
            }

            return View("ChangeContactInfo", DetermineLayout(), null);
        }

        [HttpPost]
        public ActionResult ChangeContactInfo(ChangeContactInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _volunteerSvc.UpdateDetails(new Person {
                        UserId = _webSecurity.CurrentUserId,
                        Email =  model.Email,
                        PhoneNumber = model.PhoneNumber
                    });
                    return RedirectToAction("ContactInfoChanged");
                }
                catch (PersonEmailAlreadyInUseException)
                {
                    ModelState.AddModelError("Email", "This Email Address is already in use!");
                }
            }
            return View("ChangeContactInfo", DetermineLayout(), model);
        }

        public ActionResult ContactInfoChanged()
        {
            return View();
        }

		[HttpPost]
		[AllowAnonymous]
		public bool UsernameAvailable(string userName)
		{
			return _volunteerSvc.UsernameAvailable(userName);
		}


        //
        // GET: /Account/UpgradeVolunteerToAdministrator
        [AccessDeniedAuthorize(Roles = Constants.RoleAdmin, AccessDeniedViewName = "~/Home/AccessDenied")]
        public ActionResult UpgradeVolunteerToAdministrator()
        {
            // Display list of active users so admin can select user to upgrade
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDeniedAuthorize(Roles = Constants.RoleAdmin, AccessDeniedViewName = "~/Home/AccessDenied")]
        public ActionResult UpgradeVolunteerToAdministrator(UpgradeVolunteerRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                _webSecurity.AddUserToRole(model.UserId, Constants.RoleAdmin);

                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private string DetermineLayout()
        {
            if (!_webSecurity.IsUserInRole(Constants.RoleAdmin))
                return "~/Views/Shared/_VolunteerLayout.cshtml";
            else
                return "~/Views/Shared/_AdminLayout.cshtml";
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        #endregion
    }
}
