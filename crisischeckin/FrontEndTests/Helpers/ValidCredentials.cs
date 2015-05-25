namespace FrontEndTests.Helpers
{
    internal class ValidCredentials : IValidCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }
        public string Role { get; set; }
    }
}