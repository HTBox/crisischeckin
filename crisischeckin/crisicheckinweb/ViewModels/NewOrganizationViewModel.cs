using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;
using System.Web.Mvc;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class NewOrganizationViewModel
    {

        public NewOrganizationViewModel()
        {
            PopulateTypeSelectionList();
        }

        [Required]
        [MaxLength(50)]
        public string OrganizationName { get; set; }

        public Address Address { get; set; }

        public OrganizationTypeEnum Type { get; set; }
    
        public int UserIdRegisteringOrganization { get; set; }

        public List<SelectListItem> TypeSelectionList { get; set; }

        public void PopulateTypeSelectionList()
        {
            var selectListItems = new List<SelectListItem>();

            foreach (OrganizationTypeEnum e in Enum.GetValues(typeof (OrganizationTypeEnum)))
            {
                var name = Enum.GetName(typeof (OrganizationTypeEnum), e);
                var item = new SelectListItem();
                item.Value = e.ToString();
                item.Text = name;
                selectListItems.Add(item);
            }

            this.TypeSelectionList = selectListItems;
        }
    }
}