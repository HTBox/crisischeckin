using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class VolunteerRequestIndexViewModel
    {
        public List<Request> RequestAssignedToVolunteer { get; set; }
        public IEnumerable<Request> OpenRequests { get; set; }

        // Empty Request object used for Razor templates 
        public Request Request { get; set; }
    }
}