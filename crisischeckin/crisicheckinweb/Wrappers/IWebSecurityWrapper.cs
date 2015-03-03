namespace crisicheckinweb.Wrappers
{
    public interface IWebSecurityWrapper
    {
        int CurrentUserId { get; }

        string CurrentUserName { get; }

        int GetUserId(string userName);

        string CreateUserAndAccount(string userName, string password);

        bool ChangePassword(string userName, string currentPassword, string newPassword);

        bool Login(string userName, string password, bool persistCookie = false);

        void Logout();

        bool ValidateUser(string userName, string password);

        bool IsUserInRole(string roleName);

        bool IsUserInRole(string userName, string roleName);

        void AddUserToRole(string userName, string roleName);
    }
}
