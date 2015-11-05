using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VolunteerTypesService : IVolunteerTypeService
    {
        private readonly IDataService _svc;
        public VolunteerTypesService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            _svc = service;
        }

        public IEnumerable<Models.VolunteerType> GetList()
        {
            return _svc.VolunteerTypes.OrderBy(c => c.Name).ToList();
        }
    }
}
