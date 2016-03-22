using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crisicheckinweb.ViewModels
{
    public class CheckinListsResultsViewModel
    {
        public CheckinListsResultsViewModel()
        {
            VolunteerCheckins = new List<Person>();
            ResourceCheckins = new List<Resource>();
            OrganizationContacts = new List<Contact>();
        }

        public List<Person> VolunteerCheckins { get; set; }
        public List<Resource> ResourceCheckins { get; set; }
        public List<Contact> OrganizationContacts { get; set; }
    }
}