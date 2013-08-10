using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;

namespace WebProjectTests.ServiceTests
{
    [TestClass]
    public class AdminServiceTests
    {
        //With no volunteers, return an empty list.
        [TestMethod]
        public void WhenNoVolunteersAreRegisteredReturnAnEmptyList()
        {
            var underTest = new Admin();

            var result = underTest.GetVolunteers(0);
            Assert.IsFalse(result.Any());
        }
    }
}
