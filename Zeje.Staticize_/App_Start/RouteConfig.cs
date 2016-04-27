using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Zeje.Staticize_
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "HomeListPage",
                url: "{controller}/{action}/{pageSize}/{pageIndex}.zeje",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    pageSize = UrlParameter.Optional,
                    pageIndex = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}.zeje",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Default1",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
