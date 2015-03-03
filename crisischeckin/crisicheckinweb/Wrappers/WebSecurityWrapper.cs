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

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }

        public string CreateUserAndAccount(string userName, string password)
        {
            return WebSecurity.CreateUserAndAccount(userName, password);
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            return Roles.IsUserInRole(userName, roleName);
        }

        public void AddUserToRole(string userName, string roleName)
        {
            Roles.AddUserToRole(userName, roleName);
        }
    }
}