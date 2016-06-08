using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace crisicheckinweb.Helpers
{
    public static class SortExtensions
    {
        public static MvcHtmlString SortLink(this HtmlHelper htmlHelper, string linkText, string controllerMethod)
        {
            var sortValues = new
            {
                sortField     = linkText,
                sortOrder     = htmlHelper.ViewBag.SortOrderParam,
                endDate       = htmlHelper.ViewBag.SpecifiedRequest.NullableEndDate,
                createdDate   = htmlHelper.ViewBag.SpecifiedRequest.NullableCreatedDate,
                location      = htmlHelper.ViewBag.SpecifiedRequest.Location,
                description   = htmlHelper.ViewBag.SpecifiedRequest.Description,
                requestStatus = htmlHelper.ViewBag.SpecifiedRequest.RequestStatus
            };

            var r = htmlHelper.ActionLink(linkText, controllerMethod, sortValues).ToHtmlString();
            return MvcHtmlString.Create(r);
        }
    }
}