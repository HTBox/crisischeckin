using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Breeze.Sharp;
using CrisisCheckinMobile;

namespace AcceptanceTests.Mobile
{
    // This test requires that the IIS server be running.
    [TestClass]
    public class ClientModelTest : With_an_empty_database_environment
    {
        [TestMethod]
        public async Task Mobile_ClientModel_GetDisasters()
        {
            var entityManager = new EntityManager("http://localhost:2077/Breeze/Entities");

            Breeze.Sharp.Configuration.Instance.ProbeAssemblies(typeof(ClientModel).Assembly);
            var model = new ClientModel(entityManager);
            await model.RefreshAsync();
            Assert.IsNotNull(model.Person.Commitments[0].Disaster);
        }
    }
}
