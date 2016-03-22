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

        public IEnumerable<Organization> GetActiveList()
        {
            return _dataService.Organizations.OrderBy(o => o.OrganizationName);
        }
    }
}
