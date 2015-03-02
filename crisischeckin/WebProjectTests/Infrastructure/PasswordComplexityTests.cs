using System;
using crisicheckinweb.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources;

namespace WebProjectTests.Infrastructure
{
    [TestClass]
    public class PasswordComplexityTests
    {
        [TestMethod]
        public void ValidWhenPasswordIsEqualToUsername()
        {
            // Act
            string errorMessage;
            bool valid = PasswordComplexity.IsValid("psswrd", "user01", out errorMessage);

            // Assert
            Assert.IsTrue(valid);
            Assert.IsNull(errorMessage);
        }

        [TestMethod]
        public void InvalidWhenPasswordIsEqualToUsername()
        {
            // Act
            string errorMessage;
            bool valid = PasswordComplexity.IsValid("user01", "user01", out errorMessage);

            // Assert
            Assert.IsFalse(valid);
            Assert.AreEqual(DefaultErrorMessages.InvalidPasswordEqualToUserName, errorMessage);
        }
    }
}
