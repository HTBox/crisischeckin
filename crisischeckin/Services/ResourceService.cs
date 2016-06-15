using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ResourceService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            _dataService = service;
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await _dataService.Resources.Include(r => r.Disaster)
                                               .Include(r => r.Person)
                                               .Include(r => r.ResourceType)
                                               .ToListAsync();
        }

        public Task<Resource> FindResourceByIdAsync(int resourceId)
        {
            throw new NotImplementedException();
        }

        public async Task SaveNewResourceAsync(int currentUserId, Resource resource)
        {
            resource.PersonId = currentUserId;
            resource.EntryMade = DateTime.Now;

            await _dataService.AddResourceAsync(resource);
        }

        public async Task RemoveResourceById(int resourceId)
        {
            await _dataService.RemoveResourceByIdAsync(resourceId);
        }
    }
}
