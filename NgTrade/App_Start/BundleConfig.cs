using System.Web.Optimization;

namespace NgTrade
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/assets/plugins/jquery-migrate-1.2.1.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery.color-{version}.js",
                "~/Scripts/jQueryColor-FlashEffect.js",
                "~/Scripts/jquery.signalR-{version}.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/bootstrap.*",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                        "~/Scripts/ngtrade.*"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/assets/plugins/back-to-top.js",
                "~/assets/plugins/owl-carousel/owl-carousel/owl.carousel.js",
                "~/assets/plugins/revolution_slider/rs-plugin/js/jquery.themepunch.revolution.min.js",
                "~/assets/js/app.js",
                "~/assets/js/pages/index.js",
                "~/assets/js/plugins/owl-carousel.js",
                "~/assets/plugins/gmap/gmap.js",
                "~/assets/js/pages/page_contacts.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
               "~/Scripts/angular.js",
               "~/Scripts/angular-ng-grid.js",
               "~/Scripts/angular-resource.js",
               "~/Scripts/angular-route.js",
               "~/Scripts/q.min.js",
               "~/Scripts/breeze.min.js",
               "~/Scripts/breeze.angular.js",
               "~/Scripts/breeze.savequeuing.js",
               "~/Scripts/toastr.js",
               "~/Scripts/phelper.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/app.js",
                "~/Scripts/app/services/services.js",
                "~/Scripts/app/services/directives.js",
                "~/Scripts/app/services/datacontext.js",
                "~/Scripts/app/home/main.js",
                "~/Scripts/app/home/contact.js",
                "~/Scripts/app/home/about.js",
                "~/Scripts/app/demo.js",
                "~/Scripts/app/home/static.js",
                "~/Scripts/app/daily/index.js",
                "~/Scripts/app/account/login.js",
                "~/Scripts/app/account/register.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/themes/default/bootstrap.min.css",
                        "~/Content/themes/default/bootstrap-responsive.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/unify").Include(
                "~/assets/plugins/bootstrap/css/bootstrap.css",
                "~/assets/css/style.css",
                "~/assets/plugins/line-icons/line-icons.css",
                "~/assets/plugins/font-awesome/css/font-awesome.css",
                "~/assets/plugins/revolution_slider/rs-plugin/css/settings.css",
                "~/assets/plugins/owl-carousel/owl-carousel/owl.carousel.css",
                "~/assets/css/themes/default.css",
                "~/assets/css/themes/headers/default.css",
                "~/assets/css/pages/page_contact.css",
                "~/assets/css/themes/default.css",
                "~/assets/css/custom.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/default/bootstrap.min.css",
                        "~/Content/themes/default/bootstrap-responsive.min.css",
                        "~/Content/themes/default/font-awesome.css",
                        "~/Content/themes/default/font-awesome-ie7.css",
                        "~/Content/themes/default/ngtrade.css"));


            //bundles.Add(new StyleBundle("~/Content/themes/jquerybase/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"
            //            ));
        }
    }
}