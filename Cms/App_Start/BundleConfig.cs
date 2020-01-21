using System.Web;
using System.Web.Optimization;

namespace Cms
{
    public class BundleConfig
    {
        // Aby uzyskać więcej informacji o grupowaniu, odwiedź stronę https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        //dropZone plugins
                        "~/Scripts/dropzone/dropzone.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Użyj wersji deweloperskiej biblioteki Modernizr do nauki i opracowywania rozwiązań. Następnie, kiedy wszystko będzie
            // gotowe do produkcji, użyj narzędzia do kompilowania ze strony https://modernizr.com, aby wybrać wyłącznie potrzebne testy.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/templates").Include(
                "~/Scripts/jquery-{version}.js",
                 "~/Scripts/esm/popper.js",
                 "~/Template/node_modules/bootstrap/dist/js/bootstrap.bundle.js",
                 "~/Template/front.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                          "~/Scripts/Shop/scripts.js"
                ));
            //template admin
            bundles.Add(new StyleBundle("~/Content/css").Include(

                    "~/Content/css/bootstrap.min.css",
                    "~/Content/css/bootstrap-theme.min.css",
                    "~/Content/css/PagedList.css",
                     "~/Content/css/Site.css"

                     ));

            //template css draft
            /**
             * @TODO corect vs tools node 
             */

            bundles.Add(new StyleBundle("~/Template/css").Include(
                      "~/Template/css/main.css"));
        }
    }
}
