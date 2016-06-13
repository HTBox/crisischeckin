using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Interfaces;

namespace Services
{
    public class ResourceService : IResource
    {
        private readonly IDataService _dataService;
        public ResourceService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public Task UpdateResource(Resource resource)
        {
            throw new NotImplementedException();
        }
    }
}
