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

        public IEnumerable<Disaster> Disasters { get; set; }

        public ClientModel(EntityManager entityManager)
        {
            // TODO: Load persisted data.
            entityManager.MetadataStore.NamingConvention = new NamingConvention().WithClientServerNamespaceMapping("CrisisCheckinMobile.Models", "Models");
            _entityManager = entityManager;
        }

        public async Task RefreshAsync()
        {
            var query = new EntityQuery<Person>();
            IEnumerable<Person> persons = await _entityManager.ExecuteQuery<Person>(query);
            Person = persons.FirstOrDefault();

            // TODO: Also get distastes, make it part of the same query.
        }

        public Task SaveChanges()
        {
            // TODO: Save changes to local flash.
            return _entityManager.SaveChanges(); // TODO: Look for an error and retry if there is one. An algorithm like this: http://stackoverflow.com/q/18923663/145173
        }
    }
}
