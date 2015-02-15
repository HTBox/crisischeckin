using System;
using System.Web;
using System.Web.Mvc;

namespace crisicheckinweb.Infrastructure.Attributes
{
    public class ExpireContentAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.Cache.SetLastModified(DateTime.Now);

            filterContext.HttpContext.Response.ExpiresAbsolute = DateTime.Now.AddMinutes(-1);
            filterContext.HttpContext.Response.AddHeader("Pragma", "no-cache");

            base.OnResultExecuted(filterContext);
        } 
    }
}