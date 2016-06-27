﻿using System;
using System.Collections.Generic;

using Models;

namespace Services.Interfaces
{
    public interface IOrganizationService
    {
        Organization Get(int id);
        IEnumerable<Organization> GetActiveList();
        Organization AddOrganization(Organization newOrganization, int registeringPersonId);
        void VerifyOrganization(int organizationId);
    }
}
