namespace Common
{
    public static class Constants
    {
        // Twilio Limits
        public const int TwilioMessageLength = 160;

        // Security Roles
        public const string RoleAdmin = "Admin";
        public const string RoleVolunteer = "Volunteer";

        // Default Administrator account
        public const string DefaultAdministratorUserName = "Administrator";
        public const string DefaultAdministratorPassword = "P@$$w0rd";

        // For automated tests
        public const string DefaultTestUserName = "TestUser";
        public const string DefaultTestUserPassword = "test";

        //Environments
        public const string Staging = "staging";
        public const string Development = "development";
        public const string Production = "production";
    }
}
