using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace project0_7
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            routes.MapRoute(
                name: "Customers",
                url: "Customers/Index",
                defaults: new { controller = "Customers", action = "Details" }
            );*/
            /*
            routes.MapRoute(
                name: "Transactions",
                url: "BusinessAccounts/Transactions/id"
                defaults: new {controller ="BusinessAccounts", action="Transactions", id = UrlParameter.}

            );*/
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
