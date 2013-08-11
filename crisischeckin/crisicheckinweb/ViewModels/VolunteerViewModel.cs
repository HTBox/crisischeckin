using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class VolunteerViewModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public int SelectedDisaster { get; set; }
        public IEnumerable<Disaster> MyDisasters { get; set; }
    }
}