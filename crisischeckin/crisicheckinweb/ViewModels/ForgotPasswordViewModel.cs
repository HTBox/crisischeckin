using System;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; }
    }
}