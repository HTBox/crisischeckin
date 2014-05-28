using System.Web;
using System.Web.Mvc;
using crisischeckinweb.Attributes;


namespace crisicheckinweb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
        }
    }
}