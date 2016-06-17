using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class RequestDetailsViewModel
    {
        public Request Request { get; set; }

        [DisplayName("Possible Assigniees")]
        public IEnumerable<Person> PoteintialAssigniees { get; set; } 
    }
}