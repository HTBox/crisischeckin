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

        public ResourceSearch(List<Disaster> disasters, List<Organization> organizations, List<ResourceType> resourceTypes)
        {
            var alldisasters = new Disaster()
            {
                Id = GeneralSelectId,
                IsActive = true,
                Name = "All Disasters"
            };
            disasters.Insert(0, alldisasters);

            var allOrganizations = new Organization()
            {
                OrganizationId = GeneralSelectId,
                OrganizationName = "All Organizations"
            };
            organizations.Insert(0, allOrganizations);

            var allResourceTypes = new ResourceType()
            {
                ResourceTypeId = GeneralSelectId,
                TypeName = "All Resource Types"
            };
            resourceTypes.Insert(0, allResourceTypes);

            _disasters = disasters;
            _organizations = organizations;
            _resourceTypes = resourceTypes;
        }

        public ResourceSearch(List<Disaster> disasters, List<Organization> organizations, List<ResourceType> resourceTypes, Disaster fixedDisaster, Organization fixedOrganization)
            : this(disasters, organizations, resourceTypes)
        {
            if (fixedDisaster != null)
            {
                FixedDisasterId = fixedDisaster.Id;
                FixedDisaster = fixedDisaster;
            }
            if (fixedOrganization != null)
            {
                FixedOrganizationId = fixedOrganization.OrganizationId;
                FixedOrganization = fixedOrganization;
            }
        }

        public int? FixedOrganizationId { get; set; }
        public Organization FixedOrganization { get; set; }
        public int? FixedDisasterId { get; set; }
        public Disaster FixedDisaster { get; set; }

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

        // organizations
        private readonly List<Organization> _organizations;

        [DisplayName("Selected Organization")]
        public int SelectedOrganizationId { get; set; }

        public IEnumerable<SelectListItem> OrganizationItems
        {
            get { return new SelectList(_organizations, "OrganizationId", "OrganizationName"); }
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