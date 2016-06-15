using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IResource
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<Resource> FindResourceByIdAsync(int resourceId);
        Task SaveNewResourceAsync(int currentUserId, Resource resource);
        Task RemoveResourceById(int resourceId);
    }
}
