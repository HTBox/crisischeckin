﻿using System.Web.Mvc;
using crisicheckinweb.Infrastructure;
using Common;
using crisicheckinweb.ViewModels;
using Models;
using Services.Exceptions;
using Services.Interfaces;
using crisicheckinweb.Wrappers;
using Resources;

namespace crisicheckinweb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IVolunteerService _volunteerSvc;
        private readonly ICluster _clusterSvc;
        private readonly IWebSecurityWrapper _webSecurity;

        public AccountController(IVolunteerService volunteerSvc, ICluster clusterSvc, IWebSecurityWrapper webSecurity)
        {
            _clusterSvc = clusterSvc;
            _webSecurity = webSecurity;
            _volunteerSvc = volunteerSvc;
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
            if (ModelState.IsValid && _webSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                if (_webSecurity.IsUserInRole(model.UserName, Constants.RoleAdmin))
                {
                    return RedirectToAction("List", "Disaster");
                }
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
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
            var model = new RegisterModel { Clusters = _clusterSvc.GetList() };
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
                        var userId = _webSecurity.CreateUser(
                            model.UserName,
                            model.Password,
                            new[] { Constants.RoleVolunteer });

                        _volunteerSvc.Register(model.FirstName, model.LastName, model.Email, model.PhoneNumber, model.Cluster, userId);

                        return RedirectToAction("Index", "Home");
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
            model.Clusters = _clusterSvc.GetList();
            return View(model);
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
        [Authorize(Roles = Constants.RoleAdmin)]
        public ActionResult UpgradeVolunteerToAdministrator()
        {
            // Display list of active users so admin can select user to upgrade
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.RoleAdmin)]
        public ActionResult UpgradeVolunteerToAdministrator(UpgradeVolunteerRoleViewModel model)
        {
            // Get username by selected uerId.
            //var user = 

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
