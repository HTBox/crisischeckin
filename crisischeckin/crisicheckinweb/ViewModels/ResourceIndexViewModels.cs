using System;
using System.Collections.Generic;
using crisicheckinweb.ViewModels.SearchModels;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class AdminResourceIndexViewModel
    {
        public IEnumerable<Resource> Resources { get; set; }
        public ResourceSearch ResourceSearch { get; set; }
    }

    public class VolunteerResourceIndexViewModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public int SelectedDisaster { get; set; }
        public DateTime? CommitmentDate { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public ResourceSearch ResourceSearch { get; set; }
    }
}