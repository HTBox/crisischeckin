using Breeze.Sharp;
using CrisisCheckinMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisisCheckinMobile
{
    public class ClientModel : BaseEntity
    {
        readonly EntityManager _entityManager;

        public Person Person { get; set; }

        public ClientModel(EntityManager entityManager)
        {
            entityManager.MetadataStore.NamingConvention = new NamingConvention().WithClientServerNamespaceMapping("CrisisCheckinMobile.Models", "Models");
            _entityManager = entityManager;
        }

        public async Task RefreshPersonsAsync()
        {
            var query = new EntityQuery<Person>();
            IEnumerable<Person> persons = await _entityManager.ExecuteQuery<Person>(query);
            Person = persons.FirstOrDefault();
        }
    }
}
