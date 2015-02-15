using System.Web.Mvc;
using crisicheckinweb.Infrastructure.Attributes;

namespace crisicheckinweb.Controllers
{
    [Authorize, ExpireContent]
    public class BaseController : Controller
    {
    }
}