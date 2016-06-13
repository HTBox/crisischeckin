using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using RestSharp.Extensions;

namespace crisicheckinweb.Helpers
{
    public static class LocationForExtension
    {
        public static MvcHtmlString LocationFor(this HtmlHelper htmlHelper, Address address)
        {
            string adr = address.BuildingName + "<br/>"
                         + (address.AddressLine1.HasValue() ? (address.AddressLine1 + "<br/>") : "")
                         + (address.AddressLine2.HasValue() ? (address.AddressLine2 + "<br/>") : "")
                         + (address.AddressLine3.HasValue() ? (address.AddressLine3 + "<br/>") : "")
                         + (address.City.HasValue() ? (address.City) : "")
                         + (address.County.HasValue() ? (" (" + address.County + ")") : "")
                         + ((!address.City.HasValue() && !address.County.HasValue()) ? "" : ", ")
                         + (address.State.HasValue() ? (address.State + " ") : "")
                         + (address.PostalCode.HasValue() ? (address.PostalCode + " ") : "");
            return MvcHtmlString.Create(adr);
        }
    }
}