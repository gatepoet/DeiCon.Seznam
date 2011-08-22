using System.Web.Mvc;

namespace Seznam.Controllers
{
    public class BaseController : Controller
    {
        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            base.Execute(requestContext);
        }
    }
}