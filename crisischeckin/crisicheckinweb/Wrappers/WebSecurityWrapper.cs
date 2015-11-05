using System;
using System.Web.Security;
using WebMatrix.WebData;

namespace crisicheckinweb.Wrappers
{
    public class WebSecurityWrapper : IWebSecurityWrapper
    {
        public int CurrentUserId
        {
            get { return WebSecurity.CurrentUserId; }
        }

        public string CurrentUserName
        {
            get { return WebSecurity.CurrentUserName; }
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            bool loggedIn = WebSecurity.Login(userName, password, persistCookie);
            if (!loggedIn)
            {
                if (WebSecurity.UserExists(userName) && !WebSecurity.IsConfirmed(userName))
                {
                    throw new UserNotActivatedException();
                }
            }
            return loggedIn;
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public string CreateUser(string userName, string password, string[] roleNames, out int userId)
        {
            try
            {
                var token = WebSecurity.CreateUserAndAccount(userName, password, requireConfirmationToken: true);

                //Login(userName, password);
                foreach (var roleName in roleNames)
                {
                    AddUserToRole(userName, roleName);
                }
                userId = GetUserId(userName);

                return token;
            }
            catch (MembershipCreateUserException e)
            {
                throw new UserCreationException(ErrorCodeToString(e.StatusCode));
            }
        }

        public bool ConfirmAccount(string token)
        {
            return WebSecurity.ConfirmAccount(token);
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }

        public string GeneratePasswordResetToken(string userName)
        {
            return WebSecurity.GeneratePasswordResetToken(userName);
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }

        public bool IsUserInRole(string roleName)
        {
            return Roles.IsUserInRole(roleName);
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            return Roles.IsUserInRole(userName, roleName);
        }

        public void AddUserToRole(string userName, string roleName)
        {
            Roles.AddUserToRole(userName, roleName);
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
    }
}