using MvcContrib.PortableAreas;
using Seznam.Web.Account.Controllers;

namespace Seznam.Web.Account
{
    public class ListRegistration : PortableAreaRegistration
    {
        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context, IApplicationBus bus)
        {
            context.MapRoute("AccountScript", "Account/Scripts/{resourceName}",
                             new { controller = "CachedResource", action = "Index" },
                             new[] { "Seznam.Web.Common" });
            context.MapRoute("AccountViewModels", "Account/ViewModels/{resourceName}",
                             new { controller = "CachedResource", action = "Index", resourcePath="Scripts.ViewModels" },
                             new[] { "Seznam.Web.Common" });

            context.MapRoute(
                "GetIds", // Route name
                "Account/Ids/{usernames}", // URL with parameters
                new { controller = "Account", action = "Ids" } // Parameter defaults
                );

            context.MapRoute(
                "Account",
                "Account/{action}",
                new { controller = "Account"},
                new[] { typeof(AccountController).Namespace }
            );

            RegisterAreaEmbeddedResources();
        }

        public override string AreaName
        {
            get { return "Account"; }
        }
    }
}
