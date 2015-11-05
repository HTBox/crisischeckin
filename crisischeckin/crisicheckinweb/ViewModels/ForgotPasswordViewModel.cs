using System;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        [StringLength(254)] //Email address max length
        public string UserNameOrEmail { get; set; }
    }
}