using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Seznam
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "PersonalListItem", // Route name
                "List/Details/{listName}/{name}", // URL with parameters
                new { controller = "List", action = "PersonalItemDetail"} // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{json}", // URL with parameters
                new { controller = "Home", action = "Index", json = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_BeginRequest()
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Debug.WriteLine("ERROR:");
            Debug.WriteLine("Url: " + Request.Url);
            Debug.WriteLine("Message: " + ex.Message);
            Debug.WriteLine(ex.StackTrace);
            Server.ClearError();
        }
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
        }
    }
}