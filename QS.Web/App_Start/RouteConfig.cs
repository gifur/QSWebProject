using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ArticleCategory",
                url: "Article/Category/{category}/{id}",
                defaults: new { controller = "Article", action = "Category", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "NewsCategory",
                url: "News/Category/{category}/{id}",
                defaults: new { controller = "News", action = "Category", id = UrlParameter.Optional }
                );
            routes.MapRoute(
            name: "NewsItem",
            url: "News/Item/{id}",
            defaults: new { controller = "News", action = "Item", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Search",
            url: "Search/Index/{keyword}/{pageIndex}",
            defaults: new { controller = "Search", action = "Index", pageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "News",
                url: "News/{action}/{category}",
                defaults: new { controller = "News", action = "Index", category = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "VideoCategory",
                url: "Video/Category/{category}/{id}",
                defaults: new { controller = "Video", action = "Category", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "BookCategory",
                url: "Book/Category/{category}/{id}",
                defaults: new { controller = "Book", action = "Category", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

        //定制404和500页面信息
            //routes.MapRoute("500", "500", new { controller = "SiteStatus", action = "_500" });
            //routes.MapRoute("404", "404", new { controller = "SiteStatus", action = "_404" });
        }
    }
}