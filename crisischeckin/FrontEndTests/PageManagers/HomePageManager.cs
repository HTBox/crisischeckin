using System;
using System.Collections.Generic;
using System.Linq;
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

        public void NavigateTo()
        {
            _driver.Navigate().GoToUrl(Constants.BaseUrl);
        }

        public void LogIn(ITestCredentials credentials)
        {
            var usernameField = _driver.FindElement(By.CssSelector("#UserNameOrEmail"));
            var passwordField = _driver.FindElement(By.CssSelector("#Password"));
            var loginButton =
                _driver.FindElement(By.CssSelector("body > div > div.container > div > form > div:nth-child(5) > input"));

            var errorMessages = new List<string>();

            var usernameErrors = AsssertElementIsUsable(usernameField);
            var passwordErrors = AsssertElementIsUsable(passwordField);
            var loginButtonErrors = AsssertElementIsUsable(loginButton);

            errorMessages.AddRange(usernameErrors);
            errorMessages.AddRange(passwordErrors);
            errorMessages.AddRange(loginButtonErrors);

            if (errorMessages.Any())
            {
                throw new AggregateException("Page not in expected state." +
                                             errorMessages.Select(x => Environment.NewLine + x));
            }

            usernameField.SendKeys(credentials.UserName);
            passwordField.SendKeys(credentials.Password);

            loginButton.Click();
        }

        private static IEnumerable<string> AsssertElementIsUsable(IWebElement textBox)
        {
            var errorMessages = new List<string>();

            if (!textBox.Displayed)
            {
                errorMessages.Add("Expected username textBox to be visible.");
            }

            if (!textBox.Enabled)
            {
                errorMessages.Add("Expected username textbox to be enabled.");
            }

            return errorMessages;
        }
    }
}