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

        [Test]
        public void Nonauthenticated_user_prompted_to_login()
        {
            var homePage = new HomePageManager(_driver);

            homePage.NavigateTo();
            homePage.LogIn(CredentialsStore.AdminCredentials);
        }
    }
}