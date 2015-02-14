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

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return Roles.IsUserInRole(username, roleName);
        }
    }
}