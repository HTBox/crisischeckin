using System;
using System.Web.Mvc;

namespace crisicheckinweb.Filters
{
    public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    {
        public string AccessDeniedViewName { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.User.Identity.IsAuthenticated
                && filterContext.Result is HttpUnauthorizedResult)
            {
                if (string.IsNullOrWhiteSpace(AccessDeniedViewName))
                {
                    AccessDeniedViewName = "~/Account/AccessDenied";
                }

                filterContext.Result = new RedirectResult(AccessDeniedViewName);
            }
        }
    }
}