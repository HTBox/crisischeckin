using Microsoft.VisualStudio.TestTools.UnitTesting;
using crisicheckinweb.Infrastructure.Attributes;

namespace WebProjectTests.Infrastructure.Attributes
{
    [TestClass]
    public class BasicPasswordAttributeTests
    {
        private BasicPasswordAttribute Model;

        public BasicPasswordAttributeTests()
        {
            Model = new BasicPasswordAttribute();
        }

        /// <summary>
        /// Allows for the attribute to be placed on a form, but not enforcing entry.
        /// </summary>
        [TestMethod]
        public void BlankPasswordValueIsOk()
        {
            Assert.IsTrue(Model.IsValid(string.Empty));
        }

        [TestMethod]
        public void RegularInputPasses()
        {
            Assert.IsTrue(Model.IsValid("P@55w0rd"));
        }

        [TestMethod]
        public void PasswordsWithOutsideCharactersPass()
        {
            Assert.IsTrue(Model.IsValid("PÆsswordb1"));
        }

        [TestMethod]
        public void GenericPasswordFails()
        {
            Assert.IsFalse(Model.IsValid("password"));
        }

        [TestMethod]
        public void ShortPasswordsFail()
        {
            Assert.IsFalse(Model.IsValid("short"));
        }

        [TestMethod]
        public void LongPasswordsFail()
        {
            Assert.IsFalse(Model.IsValid("ThisPasswordIsALittleLong"));
        }

    }
}