using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Breeze.Sharp;
using CrisisCheckinMobile;
using Common;
using System.Text;
using System.Configuration;

namespace AcceptanceTests.Mobile
{
    // This test requires that the IIS server be running at the endpoint specified in the config.
    [TestClass]
    public class ClientModelTest
    {
        [TestMethod]
        public async Task Mobile_ClientModel_GetPersonWithDisaster()
        {
            try
            {
                var entityManager = GetInitializedEntityManager();

                Breeze.Sharp.Configuration.Instance.ProbeAssemblies(typeof(ClientModel).Assembly);
                var model = new ClientModel(entityManager);
                await model.RefreshAsync();
                Assert.IsNotNull(model.Person.Commitments[0].Disaster);
            }
            catch (Exception ex)
            {
                var e = ex;
                Assert.Fail(ex.Message);
            }
        }



        private static EntityManager GetInitializedEntityManager()
        {
            var endpointUrl = ConfigurationManager.AppSettings["apiEndpointUrl"];
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes((Constants.DefaultTestUserName + "|" + Constants.DefaultTestUserPassword).ToCharArray()));

            var breezeDataService = new DataService(endpointUrl);
            breezeDataService.HttpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
            var entityManager = new EntityManager(breezeDataService);
            return entityManager;
        }
    }
}
