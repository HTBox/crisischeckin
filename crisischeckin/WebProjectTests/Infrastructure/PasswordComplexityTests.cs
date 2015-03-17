using System;
using crisicheckinweb.Infrastructure;
using NUnit.Framework;
using Resources;

namespace WebProjectTests.Infrastructure
{
    [TestFixture]
    public class PasswordComplexityTests
    {
        [Test]
        public void ValidWhenPasswordIsEqualToUsername()
        {
            // Act
            string errorMessage;
            bool valid = PasswordComplexity.IsValid("psswrd", "user01", out errorMessage);

            // Assert
            Assert.IsTrue(valid);
            Assert.IsNull(errorMessage);
        }

        [Test]
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
