namespace FrontEndTests.Helpers
{
    internal static class CredentialsStore
    {
        public static ITestCredentials AdminCredentials { get; } = new TestCredentials
        {
            UserName = "Administrator",
            Password = "P@$$w0rd"
        };
    }
}