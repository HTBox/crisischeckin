using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class MenuExtensions
{
    public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string text, string action, string controller, object routeValues = null, object htmlAttributes = null)
    {
        var li = new TagBuilder("li");
        var routeData = htmlHelper.ViewContext.RouteData;
        var currentAction = routeData.GetRequiredString("action");
        var currentController = routeData.GetRequiredString("controller");
        if (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) && string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
        {
            li.AddCssClass("active");
        }
        if (routeValues != null)
        {
            li.InnerHtml = (htmlAttributes != null)
                ? htmlHelper.ActionLink(text,
                                        action,
                                        controller,
                                        routeValues,
                                        htmlAttributes).ToHtmlString()
                : htmlHelper.ActionLink(text,
                                        action,
                                        controller,
                                        routeValues).ToHtmlString();
        }
        else
        {
            li.InnerHtml = htmlHelper.ActionLink(text,
                                                 action,
                                                 controller).ToHtmlString();
        }
        return MvcHtmlString.Create(li.ToString());
    }
}