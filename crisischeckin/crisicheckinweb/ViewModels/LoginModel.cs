using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        [StringLength(254)] //Email address max length
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}