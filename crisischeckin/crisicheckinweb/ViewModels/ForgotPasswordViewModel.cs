using System;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }
}