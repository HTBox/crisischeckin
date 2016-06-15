using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models;

namespace crisicheckinweb.ViewModels.SearchModels
{
    public class ResourceSearch : Resource
    {
        public const int GeneralSelectId = -100;
        public ResourceSearch()
        {
        }

        public ResourceSearch(List<Disaster> disasters, List<ResourceType> resourceTypes)
        {
            var alldisasters = new Disaster()
            {
                Id = GeneralSelectId,
                IsActive = true,
                Name = "All Disasters"
            };
            disasters.Insert(0, alldisasters);

            var allResourceTypes = new ResourceType()
            {
                ResourceTypeId = GeneralSelectId,
                TypeName = "All Resource Types"
            };
            resourceTypes.Insert(0, allResourceTypes);

            _disasters = disasters;
            _resourceTypes = resourceTypes;
        }

        [DisplayName("Availability Start")]
        public DateTime? NullableStartDate { get; set; }

        [DisplayName("Availability End")]
        public DateTime? NullableEndDate { get; set; }

        [DisplayName("Date Created")]
        public DateTime? NullableCreatedDate { get; set; }

        // disasters
        private readonly List<Disaster> _disasters;

        [DisplayName("Selected Disaster")]
        public int SelectedDisasterId { get; set; }

        public IEnumerable<SelectListItem> DisasterItems
        {
            get { return new SelectList(_disasters, "Id", "Name"); }
        }

        // resource types
        private readonly List<ResourceType> _resourceTypes;

        [DisplayName("Resource Types")]
        public int SelectedResourceTypeId { get; set; }

        public IEnumerable<SelectListItem> ResourceItems
        {
            get { return new SelectList(_resourceTypes, "ResourceTypeId", "TypeName"); }
        }
    }
}