using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace crisicheckinweb.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        [Display(Name = "User name")]
        private string userName;
        public string UserName { get { return userName; } set { userName = value.Trim(); } }
      

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Cluster")]
        public int Cluster { get; set; }

        [Required]
        [Display(Name = "Volunteer Type")]
        public int VolunteerType { get; set; }

        public IEnumerable<Cluster> Clusters { get; set; }
        public IEnumerable<VolunteerType> VolunteerTypes { get; set; }
    }
}
