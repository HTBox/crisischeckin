using FrontEndTests.Helpers;
using OpenQA.Selenium;

namespace FrontEndTests.PageManagers
{
    public class HomePageManager
    {
        private readonly IWebDriver _driver;

        public HomePageManager(IWebDriver driver)
        {
            _driver = driver;
        }

        public AccountLoginManager NavigateToAsUnauthenticated()
        {
            _driver.Navigate().GoToUrl(Constants.BaseUrl);
            var loginManager = new AccountLoginManager(_driver);
            return loginManager;
        }
    }
}