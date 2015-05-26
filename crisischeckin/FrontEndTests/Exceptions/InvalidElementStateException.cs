using System;
using OpenQA.Selenium;

namespace FrontEndTests.Exceptions
{
    public class InvalidElementStateException : Exception
    {
        public InvalidElementStateException(IWebElement element, string expectedState, string actualState)
            : base(
                $"Element in invalid state.  '{element}'. Expected state: {expectedState} Actual state: {actualState}")
        {
        }
    }
}