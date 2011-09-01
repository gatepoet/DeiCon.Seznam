using System.Web.Mvc;
using Seznam.List;
using Seznam.List.Contracts;
using Seznam.Web.Common;
using Seznam.Web.List.Models;

namespace Seznam.Web.List.Controllers
{
    public class HomeController : Controller
    {
        protected override void Dispose(bool disposing)
        {
            _listService.Dispose();
            base.Dispose(disposing);
        }
        private readonly IListService _listService;
        private readonly ISessionContext _sessionContext;
        private ILogger _logger;
        private Bus _bus;

        public HomeController()
        {
            _logger = new NullLogger();
            _listService = new ListService();
            _sessionContext = SessionContext.Current;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _bus = new Bus(Url.Content("~/request.ashx"));
        }
        public HomeController(IListService listService, ISessionContext sessionContext, ILogger logger)
        {
            _listService = listService;
            _sessionContext = sessionContext;
            _logger = logger;
        }
        [HttpGet] public ViewResult Index()     { return View(); }
        [HttpGet] public ViewResult Templates() { return View(); }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult Summary()
        {
            var user = _listService.GetSummary(_sessionContext.UserId);

            return DataResponse.Success(user);
        }

    }

}
