using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.UnitTest
{
    [TestClass]
    public class DisasterServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_BeginDateGreaterThanEndDate()
        {
            DisasterService service = new DisasterService();
            service.AssignToVolunteer(1, 1, new DateTime(2013, 6, 13), new DateTime(2013, 5, 10));
        }

        [TestMethod]
        public void AssignToVolunteer_Valid()
        {
            DisasterService service = new DisasterService();
            service.AssignToVolunteer(1, 1, new DateTime(2013, 01, 01), new DateTime(2013, 02, 01));
        }
    }
}
