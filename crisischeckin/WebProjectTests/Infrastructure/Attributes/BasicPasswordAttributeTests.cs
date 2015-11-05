using NUnit.Framework;
using crisicheckinweb.Infrastructure.Attributes;

namespace WebProjectTests.Infrastructure.Attributes
{
    [TestFixture]
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
        [Test]
        public void BlankPasswordValueIsOk()
        {
            Assert.IsTrue(Model.IsValid(string.Empty));
        }

        [Test]
        public void RegularInputPasses()
        {
            Assert.IsTrue(Model.IsValid("P@55w0rd"));
        }

        [Test]
        public void PasswordsWithOutsideCharactersPass()
        {
            Assert.IsTrue(Model.IsValid("PÆsswordb1"));
        }

        [Test]
        public void GenericPasswordFails()
        {
            Assert.IsFalse(Model.IsValid("password"));
        }

        [Test]
        public void ShortPasswordsFail()
        {
            Assert.IsFalse(Model.IsValid("short"));
        }

        [Test]
        public void LongPasswordsFail()
        {
            Assert.IsFalse(Model.IsValid("ThisPasswordIsALittleLong"));
        }

    }
}