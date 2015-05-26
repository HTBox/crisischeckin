namespace FrontEndTests.Helpers
{
    public interface IValidCredentials : ICredentials
    {
        string Role { get; }
    }
}