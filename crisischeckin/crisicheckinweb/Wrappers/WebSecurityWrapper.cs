using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace crisicheckinweb.Wrappers
{
    public class WebSecurityWrapper : IWebSecurityWrapper
    {
        public int CurrentUserId { get { return WebSecurity.CurrentUserId; } }
    }
}