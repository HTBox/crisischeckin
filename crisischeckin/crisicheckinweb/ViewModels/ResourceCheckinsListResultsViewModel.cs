using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class ResourceCheckinsListResultsViewModel
    {
        public ResourceCheckinsListResultsViewModel()
        {
            ResourceCheckins = new List<Resource>();
        }

        public List<Resource> ResourceCheckins { get; set; }
    }
}