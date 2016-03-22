using System;
using System.Collections.Generic;

using Models;

namespace Services.Interfaces
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetActiveList();
    }
}
