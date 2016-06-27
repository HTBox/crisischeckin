using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Models;

namespace crisicheckinweb.ViewModels
{
    public class OrganizationHomeViewModel
    {
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public IEnumerable<Person> Volunteers { get; set; }

        public IEnumerable<Resource> OrganizationResources { get; set; }

        public IEnumerable<OrganizationDisasterViewModel> Disasters { get; set; }

        public bool IsAdministrator { get; set; }
        public bool IsOrgAdmin { get; set; }
        public bool IsAdmin
        {
            get
            {
                return IsAdministrator || IsOrgAdmin;
            }
        }


        public IEnumerable<Disaster> AllDisasters { get; set; }
        public IEnumerable<SelectListItem> VolunteersSelectList { get; set; }


        [DisplayName("Disaster")]
        public int ResourceDisasterId { get; set; }

        [DisplayName("Description")]
        [Required]
        public string ResourceDescription { get; set; }

        [DisplayName("Quantity")]
        public int ResourceQuantity { get; set; }

        [DisplayName("Resource Type")]
        public int ResourceTypeId { get; set; }

        [DisplayName("Activity")]
        public int ResourceClusterId { get; set; }

        [DisplayName("Start Date")]
        public DateTime ResourceStartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime ResourceEndDate { get; set; }

        [DisplayName("Location")]
        public string ResourceLocation { get; set; }


        public int RemoveResourceId { get; set; }


        public int AddVolunteerId { get; set; }
        public int RemoveVolunteerId { get; set; }

        public int PromoteToAdminPersonId { get; set; }
        public int DemoteFromAdminPersonId { get; set; }


        public IEnumerable<ResourceType> ResourceTypes { get; set; }
    }
}