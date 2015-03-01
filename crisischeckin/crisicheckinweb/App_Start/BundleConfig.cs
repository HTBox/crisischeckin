using System.Web.Optimization;

namespace crisicheckinweb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/base/jquery.ui.all.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/htbox-theme.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                "~/Scripts/jquery-2.1.1.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery-ui-1.10.4.js",
                "~/Scripts/bootstrap.js"));

        }
    }
}