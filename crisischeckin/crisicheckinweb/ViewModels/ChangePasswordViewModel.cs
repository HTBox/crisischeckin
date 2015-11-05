using System.ComponentModel.DataAnnotations;
using crisicheckinweb.Infrastructure.Attributes;

namespace crisicheckinweb.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Old Password: ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        public string OldPassword { get; set; }

        [Display(Name = "New Password: ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [BasicPassword]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}