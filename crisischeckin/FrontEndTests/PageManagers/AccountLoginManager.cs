using System;
using FrontEndTests.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FrontEndTests.PageManagers
{
    public class AccountLoginManager
    {
        private readonly IWebDriver _driver;
        private const string LoginFormCssSelector = "#account-login-form";
        private const string LoginButtonCssSelector = "#submit-login-credentials";
        private const string UsernameFieldCssSelector = "#UserNameOrEmail";
        private const string PasswordFieldCssSelector = "#Password";

        private IWebElement _loginButton;
        private IWebElement _usernameField;
        private IWebElement _passwordField;

        public AccountLoginManager(IWebDriver driver)
        {
            _driver = driver;
            EnsurePageState();
        }

        private void EnsurePageState()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));

            wait.Until(driver => driver.FindElement(By.CssSelector(LoginFormCssSelector)));
            _loginButton = wait.Until(driver => driver.FindElement(By.CssSelector(LoginButtonCssSelector)));
            _usernameField = wait.Until(driver => driver.FindElement(By.CssSelector(UsernameFieldCssSelector)));
            _passwordField = wait.Until(driver => driver.FindElement(By.CssSelector(PasswordFieldCssSelector)));
        }

        public DisasterListManager LogInAdministrator()
        {
            LogInWithDetails(CredentialsStore.AdminCredentials);

            return new DisasterListManager(_driver);
        }

        private void LogInWithDetails(ICredentials credentials)
        {
            _usernameField.SendKeys(credentials.UserName);
            _passwordField.SendKeys(credentials.Password);
            _loginButton.Click();
        }

        public void LogInInvalidUser()
        {
            LogInWithDetails(CredentialsStore.UnregisteredUser);
        }
    }
}