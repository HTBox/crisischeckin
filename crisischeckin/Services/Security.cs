using System.Web.Security;
using Services.Interfaces;
using WebMatrix.WebData;

namespace Services
{
    public class Security : ISecurity
    {
        public void CreateUser(string username, string password)
        {
            WebSecurity.CreateUserAndAccount(username, password);
        }

        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            WebSecurity.ChangePassword(username, oldPassword, newPassword);
        }

        public bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        public bool Login(string username, string password, bool shouldPersist = false)
        {
            return WebSecurity.Login(username, password, shouldPersist);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool IsUserInRole(string username, string role)
        {
            return Roles.IsUserInRole(username, role);
        }

        public void AddUserToRole(string username, string role)
        {
            Roles.AddUserToRole(username, role);
        }

        public UserInfo GetInfoFor(string username)
        {
            var id = WebSecurity.GetUserId(username);

            return new UserInfo {Id = id, Username = username};
        }

        public UserInfo GetInfoForCurrentUser()
        {
            int id = WebSecurity.CurrentUserId;
            string username = WebSecurity.CurrentUserName;

            return new UserInfo {Id = id, Username = username};
        }
    }
}