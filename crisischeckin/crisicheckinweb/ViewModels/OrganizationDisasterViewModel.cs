using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class OrganizationDisasterViewModel
    {
        public DisasterViewModel Disaster { get; set; }

        public IEnumerable<Commitment> Commitments { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}