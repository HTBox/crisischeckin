using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ChangeContactInfoViewModel
    {
        [Display(Name = "Email Address:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Display(Name = "Phone Number:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string PhoneNumber { get; set; }

        public IEnumerable<Organization> Organizations { get; set; }

        [Display(Name = "Organization")]
        public int? SelectedOrganizationId { get; set; }
    }
}