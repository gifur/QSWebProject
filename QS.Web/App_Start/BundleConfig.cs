using System.Web;
using System.Web.Optimization;

namespace QS.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/assets/plugins/jquery-migrate-1.2.1.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/jquery.pagination.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                        "~/assets/plugins/bootstrap/js/bootstrap.js",
                        "~/assets/plugins/hover-dropdown.js",
                        "~/assets/plugins/back-to-top.js",
                        "~/assets/plugins/uniform/jquery.uniform.js",
                        "~/assets/scripts/bootbox.js",
                        "~/assets/plugins/fancybox/source/jquery.fancybox.pack.js"));

            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                        "~/assets/scripts/app.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/knockout.validation.js"));

            //全局样式
            bundles.Add(new StyleBundle("~/assets/plugins/css").Include(
                //"~/assets/plugins/font-awesome/css/font-awesome.css",
                "~/assets/plugins/bootstrap/css/bootstrap.css",
                "~/assets/plugins/fancybox/source/jquery.fancybox.css",
                "~/assets/plugins/uniform/css/uniform.default.css"));
            //结束全局样式

            //主题样式
            bundles.Add(new StyleBundle("~/assets/themes/css").Include(
                        "~/assets/css/style-metronic.css",
                        "~/assets/css/style.css",
                        "~/assets/css/style-addition.css",
                        "~/assets/css/style-responsive.css",
                        "~/assets/css/custom.css"));
            //结束主题样式

            //用于后台管理区域的样式和Javascript
            bundles.Add(new ScriptBundle("~/bundles/admin/script").Include(
            "~/Areas/assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.js",
            "~/Areas/assets/plugins/bootstrap/js/bootstrap.js",
            "~/Areas/assets/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.js",
            "~/Areas/assets/plugins/jquery-slimscroll/jquery.slimscroll.js",
            "~/Areas/assets/plugins/jquery.blockui.js",
            "~/Areas/assets/plugins/uniform/jquery.uniform.js",
            "~/Areas/assets/plugins/bootbox/bootbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/core").Include(
            "~/Areas/assets/scripts/core/app.js",
            "~/Areas/assets/scripts/custom/tasks.js",
            "~/Areas/assets/scripts/custom/portlet-draggable.js"));

            //全局样式
            bundles.Add(new StyleBundle("~/areas/assets/plugins/css").Include(
                //"~/Areas/assets/plugins/font-awesome/css/font-awesome.css",
                "~/Areas/assets/plugins/bootstrap/css/bootstrap.css",
                "~/Areas/assets/plugins/uniform/css/uniform.default.css"));
            //结束全局样式

            //主题样式
            bundles.Add(new StyleBundle("~/areas/assets/themes/css").Include(
                        "~/Areas/assets/css/style-metronic.css",
                        "~/Areas/assets/css/style.css",
                        "~/Areas/assets/css/style-responsive.css",
                        "~/Areas/assets/css/plugins.css",
                        "~/Areas/assets/css/pages/tasks.css",
                        "~/Areas/assets/css/custom.css"));
            //结束主题样式
        }
    }
}