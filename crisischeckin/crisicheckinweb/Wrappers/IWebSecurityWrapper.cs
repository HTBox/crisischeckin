namespace crisicheckinweb.Wrappers
{
    public interface IWebSecurityWrapper
    {
        int CurrentUserId { get; }

        bool Login(string userName, string password, bool persistCookie = false);

        bool IsUserInRole(string username, string roleName);
    }
}
