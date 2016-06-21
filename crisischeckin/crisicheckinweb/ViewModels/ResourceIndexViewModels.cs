using System.Collections.Generic;
using crisicheckinweb.ViewModels.SearchModels;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class AdminResourceIndexViewModel
    {
        public IEnumerable<ResourceCrudViewModel> Resources { get; set; }
        public ResourceSearch ResourceSearch { get; set; }
    }
}