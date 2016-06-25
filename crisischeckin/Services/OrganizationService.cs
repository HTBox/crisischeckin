using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IDataService _dataService;

        public OrganizationService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            _dataService = service;
        }

        public Organization Get(int id)
        {
            return _dataService.Organizations.SingleOrDefault(org => org.OrganizationId.Equals(id));
        }

        public IEnumerable<Organization> GetActiveList()
        {
            return _dataService.Organizations.Where(x => x.Verified).OrderBy(o => o.OrganizationName);
        }

        public Organization AddOrganization(Organization newOrganization, int registeringPersonId)
        {
            return _dataService.AddOrganization(newOrganization, registeringPersonId);
        }

        public void VerifyOrganization(int organizationId)
        {
            _dataService.VerifyOrganization(organizationId);
        }
    }
}
