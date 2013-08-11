using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crisicheckinweb.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}