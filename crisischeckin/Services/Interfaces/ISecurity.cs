namespace Services.Interfaces
{
    public interface ISecurity
    {
        void CreateUser(string username, string password);
        void ChangePassword(string username, string oldPassword, string newPassword);
        bool ValidateUser(string username, string password);
        bool Login(string username, string password, bool shouldPersist = false);
        void Logout();
        bool IsUserInRole(string username, string role);
        void AddUserToRole(string username, string role);
        UserInfo GetInfoFor(string username);
        UserInfo GetInfoForCurrentUser();
    }
}