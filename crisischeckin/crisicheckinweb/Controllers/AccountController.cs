using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using Common;
using crisicheckinweb.ViewModels;
using Models;
using Services.Exceptions;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IVolunteerService _volunteerSvc;
        private readonly ICluster _clusterSvc;
        private readonly ISecurity _securitySvc;
        private readonly IPrincipal _currentUser;

        public AccountController(IVolunteerService volunteerSvc, ICluster clusterSvc, ISecurity securitySvc, IPrincipal currentUser)
        {
            _clusterSvc = clusterSvc;
            _securitySvc = securitySvc;
            _currentUser = currentUser;
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
            if (ModelState.IsValid && _securitySvc.Login(model.UserName, model.Password, shouldPersist: model.RememberMe))
            {
                if (_securitySvc.IsUserInRole(model.UserName, Constants.RoleAdmin))
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
            _securitySvc.Logout();

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
                    _securitySvc.CreateUser(model.UserName, model.Password);
                    _securitySvc.Login(model.UserName, model.Password);

                    _securitySvc.AddUserToRole(model.UserName, Constants.RoleVolunteer);

                    var userId = _securitySvc.GetInfoFor(model.UserName).Id;

                    Person newPerson = _volunteerSvc.Register(model.FirstName, model.LastName, model.Email, model.PhoneNumber, model.Cluster, userId);

                    _volunteerSvc.UpdateDetails(newPerson);

                    return RedirectToAction("Index", "Home");
                }
                catch (PersonAlreadyExistsException)
                {
                    ModelState.AddModelError("", "Email is already in use!");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            model.Clusters = _clusterSvc.GetList();
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_securitySvc.ValidateUser(User.Identity.Name, model.OldPassword))
                {
                    _securitySvc.ChangePassword(_currentUser.Identity.Name, model.OldPassword, model.NewPassword);
                    return RedirectToAction("PasswordChanged");
                }
                ModelState.AddModelError("OldPassword", "Old password is not correct.");
            }
            return View();
        }

        public ActionResult PasswordChanged()
        {
            return View();
        }

        public ActionResult ChangeContactInfo()
        {
            int idOfCurrentUser = _securitySvc.GetInfoForCurrentUser().Id;

            if (idOfCurrentUser == 1)
            {
                TempData["AdminContactError"] = "Administrator is not allowed to have contact details!";
                return RedirectToAction("Index", "Home");
            }
            var personToUpdate = _volunteerSvc.FindByUserId(idOfCurrentUser);
            ChangeContactInfoViewModel model = new ChangeContactInfoViewModel { Email = personToUpdate.Email, PhoneNumber = personToUpdate.PhoneNumber };
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeContactInfo(ChangeContactInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var personToUpdate = _volunteerSvc.FindByUserId(_securitySvc.GetInfoForCurrentUser().Id);
                try
                {
                    _volunteerSvc.UpdateDetails(new Person {
                        Id = personToUpdate.Id,
                        Cluster= personToUpdate.Cluster,
                        ClusterId = personToUpdate.ClusterId,
                        FirstName = personToUpdate.FirstName,
                        LastName = personToUpdate.LastName,
                        UserId = personToUpdate.UserId,
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
            return View();
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
                _securitySvc.AddUserToRole(model.UserId, Constants.RoleAdmin);

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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }



        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

    }
}
