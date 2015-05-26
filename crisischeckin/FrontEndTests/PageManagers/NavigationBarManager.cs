using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FrontEndTests.PageManagers
{
    public class NavigationBarManager
    {
        private readonly IWebDriver _driver;
        private readonly string _username;
        private IWebElement _homeButton;
        private IWebElement _logoutButton;
        private const string NavBarCssSelector = "#nav-bar";
        private const string HomeButtonSCssSelector = "#home";
        private const string ListByDisasterButtonCssSelector = "#list-by-disaster";
        private const string LogoutButtonCssSelector = "#logout";

        public NavigationBarManager(IWebDriver driver, string username = null)
        {
            _driver = driver;
            _username = username;
        }

        private void EnsureState()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));

            var navBar = wait.Until(driver => driver.FindElement(By.CssSelector(NavBarCssSelector)));
           _homeButton =
                wait.Until(driver => driver.FindElement(By.CssSelector(HomeButtonSCssSelector)));

            _logoutButton =
                wait.Until(driver => driver.FindElement(By.CssSelector(LogoutButtonCssSelector)));
        }

        public HomePageManager LogOut()
        {
            _logoutButton.Click();
            return new HomePageManager(_driver);
        }
    }
}