namespace FrontEndTests.Helpers
{
    internal static class CredentialsStore
    {
        public static IValidCredentials AdminCredentials { get; } = new ValidCredentials
        {
            UserName = "Administrator",
            Password = "P@$$w0rd",
            Role = "Administrator"
        };

        public static ICredentials UnregisteredUser { get; } = new ValidCredentials
        {
            UserName = "Unknown-user",
            Password = "something"
        };
    }
}