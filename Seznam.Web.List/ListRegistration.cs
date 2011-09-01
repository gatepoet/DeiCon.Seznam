using MvcContrib.PortableAreas;
using Seznam.Web.List.Controllers;

namespace Seznam.Web.List
{
    public class ListRegistration : PortableAreaRegistration
    {
        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context, IApplicationBus bus)
        {
            context.MapRoute("ListScriptRoute", "List/Scripts/{resourceName}",
                             new {controller = "CachedResource", action = "Index"},
                             new[] { "Seznam.Web.Common" });
            context.MapRoute("ListViewModels", "List/ViewModels/{resourceName}",
                  new { controller = "CachedResource", action = "Index", resourcePath = "Scripts.ViewModels" },
                  new[] { "Seznam.Web.Common" });

            context.MapRoute(
                "ListBase",
                "List/Summary",
                new { controller = "Home", action="Summary" },
                new[] { typeof(HomeController).Namespace }
            );

            context.MapRoute(
                "List",
                "List/{controller}/{action}",
                new { controller="Home", action = "Index" },
                new []{typeof(HomeController).Namespace}
            );
            RegisterAreaEmbeddedResources();
        }

        public override string AreaName
        {
            get { return "List"; }
        }
    }
}
