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
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _dataService = service;
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await _dataService.GetAllResourcesAsync();
        }

        public async Task<Resource> FindResourceByIdAsync(int? resourceId)
        {
            return await _dataService.FindResourceByIdAsync(resourceId);
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

        public Task<IEnumerable<Resource>> GetResourcesByDisasterAsync(int selectedDisaster, DateTime? commitmentDate)
        {
            return _dataService.GetResourcesByDisasterAsync(selectedDisaster, commitmentDate);
        }
    }
}
