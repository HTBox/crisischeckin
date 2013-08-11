using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace crisicheckinweb.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Old Password: ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password: ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "Must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}