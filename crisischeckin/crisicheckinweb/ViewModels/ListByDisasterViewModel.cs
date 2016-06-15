using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crisicheckinweb.ViewModels
{
    public class ListByDisasterViewModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public int SelectedDisaster { get; set; }
        public DateTime? CommitmentDate { get; set; }


        public int AddContactId { get; set; }
    }
}