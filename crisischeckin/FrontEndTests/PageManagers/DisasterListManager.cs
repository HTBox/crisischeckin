using OpenQA.Selenium;

namespace FrontEndTests.PageManagers
{
    public class DisasterListManager
    {
        private readonly IWebDriver _driver;

        public DisasterListManager(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}