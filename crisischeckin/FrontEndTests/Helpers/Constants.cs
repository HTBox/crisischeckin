using System.Net;

namespace FrontEndTests.Helpers
{
    internal static class Constants
    {
        public const string FrontEnd = "Front End";
        public const string BaseUrl = "http://localhost:2077/";

        public static ITestCredentials AdminCredentials { get; } = new TestCredentials
        {
            UserName = "Administrator",
            Password = "P@$$w0rd"
        };
    }
}