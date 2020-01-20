using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cms
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { Controller = "Pages",action = "Index"},
                namespaces: new[] {"Cms.Controllers"}
                );

            //routing dla Pozostałych stron - Page
            routes.MapRoute("Pages","{page}", new { controller="Pages", action="Index"},new[] { "Cms.Controllers" });
            //Routing Partial - menu 
            routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "Cms.Controllers" });

            // routes.MapRoute(
            //    
            //      name: "Default",
            //     url: "{controller}/{action}/{id}",
            //     defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //       
            //   );
        }
    }
}
