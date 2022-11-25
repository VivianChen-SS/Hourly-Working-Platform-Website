using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace final_project
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            //routes.MapRoute(
            //    name: "UnfinishedEvent_Details",
            //    url: "{controller}/{action}/{id}/{showHires}/{noApplier}",
            //    defaults: new
            //    {
            //        controller = "UnfinishedEvent",
            //        action = "Details",
            //        id = UrlParameter.Optional,
            //        showHires = UrlParameter.Optional,
            //        noApplier = UrlParameter.Optional
            //    }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "AllWorksIndex",
            //    url: "Employer/AllWorksIndex/{sortOrder?}/{currentFilter?}/{searchstring?}/{clearSearchButton?}/{page?}/{onePage?}",
            //    defaults: new { Controller = "Employer"}
            //);
        }
    }
}
