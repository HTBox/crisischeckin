using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    [TestClass]
    public class A_person_can_be_unassigned_as_a_cluster_coordinator_for_a_disaster : With_an_empty_database_environment
    {
        [TestInitialize]
        public void Arrange()
        {
            //Create a disaster
            //Create a person
            //Assign a person as a cluster coordinator
        }

        [TestMethod]
        public void Act()
        {
            //Unassign the coordinator
            //Retrieve the list of coordinators for the disaster
            //Assert that the person is NOT in the list of coordinators for the cluster
            //Assert that the log contains an unassigned event
        }

        [TestCleanup]
        public void TearDown()
        {
            //Remove the log entry
            //Remove the person
            //Remove the disaster
        }
    }
}