using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using crisicheckinweb.Infrastructure.Attributes;

namespace crisicheckinweb.ViewModels
{
    public class RegisterModel
    {
        private string _userName;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public IEnumerable<Organization> Organizations { get; set; }

        [Display(Name = "Organization")]
        public int? SelectedOrganizationId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{0,15}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string UserName { get { return _userName; } set { _userName = value != null ? value.Trim() : value; } }

        [Required]
        [DataType(DataType.Password)]
        [BasicPassword]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
