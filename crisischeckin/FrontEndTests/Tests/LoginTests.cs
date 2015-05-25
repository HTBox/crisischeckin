using FrontEndTests.Helpers;
using FrontEndTests.PageManagers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FrontEndTests.Tests
{
    [TestFixture(Category = Constants.FrontEnd)]
    internal class LoginTests
    {
        private IWebDriver _driver;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _driver = new ChromeDriver();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _driver.Close();
            _driver.Quit();
        }


        [TearDown]
        public void TearDown()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
        }

        [Test]
        public void Administrator_logging_is_redirected_to_disaster_list()
        {
            var homePage = new HomePageManager(_driver);

            var loginPage = homePage.NavigateToAsUnauthenticated();
            loginPage.LogInAdministrator();
        }

        [Test]
        public void Invalid_user_sees_error_message()
        {
            var homepage = new HomePageManager(_driver);
            var loginPage = homepage.NavigateToAsUnauthenticated();
            loginPage.LogInInvalidUser();
        }
    }
}