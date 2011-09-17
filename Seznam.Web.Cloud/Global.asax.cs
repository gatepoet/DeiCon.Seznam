using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.UI.InputBuilder;
using NLog;
using Seznam.Web.Controllers;

namespace Seznam.Web
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
            routes.IgnoreRoute("{resource}.ashx");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{json}", // URL with parameters
                new { controller = "Home", action = "Index", json = UrlParameter.Optional },
                new[] { typeof(HomeController).Namespace }

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
            Debug.WriteLine("Message: " + ex);
            Debug.WriteLine(ex.StackTrace);
            Server.ClearError();
            var logger = new LogFactory().GetLogger("file");
            logger.Error(ex);
        }
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            InputBuilder.BootStrap();

            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }
    }
}