using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crisicheckinweb.Controllers
{
    [Authorize]
    public class BaseAuthorizedController : Controller
    {
    }
}