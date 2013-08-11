using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace crisicheckinweb.ViewModels
{
    public class UpgradeVolunteerRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

    }
}