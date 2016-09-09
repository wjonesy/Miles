using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Miles.Sample.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Teams",
                url: "Teams/{action}/{id}",
                defaults: new { controller = "Teams", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Leagues",
                url: "Leagues/{id}/{action}",
                defaults: new { controller = "Leagues", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Fixtures",
                url: "Leagues/{leagueId}/Fixtures/{action}/{fixtureId}",
                defaults: new { controller = "Fixtures", action = "Index", fixtureId = UrlParameter.Optional }
            );
        }
    }
}
