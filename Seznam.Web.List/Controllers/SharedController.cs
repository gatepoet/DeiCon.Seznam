using System.Web.Mvc;
using Seznam.List;
using Seznam.List.Contracts;
using Seznam.Web.Common;
using Seznam.Web.List.Models;
using Seznam.Web.List.Models.Messages;

namespace Seznam.Web.List.Controllers
{
    public class SharedController : Controller
    {
        protected override void Dispose(bool disposing)
        {
            _listService.Dispose();
            base.Dispose(disposing);
        }
        private readonly IListService _listService;
        private readonly ISessionContext _sessionContext;
        private Bus _bus;

        public SharedController()
        {
            _listService = new ListService();
            _sessionContext = SessionContext.Current;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _bus = new Bus(Url.Content("~/request.ashx"));
        }
        public SharedController(IListService listService, ISessionContext sessionContext)
        {
            _listService = listService;
            _sessionContext = sessionContext;
        }

        [HttpGet] public ActionResult Index() { return View(); }
        [HttpGet] public ActionResult Detail() { return View(); }



        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void Toggle(ToggleData td)
        {
            var data = _listService.ToggleSharedItem(td.ListId, td.ItemName, td.ItemCompleted);

            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemToggledMessage(data.Item, _sessionContext.Username), data.List.Users);
            }
            _bus.Publish(new ItemToggledMessage(data.Item, _sessionContext.Username), data.List.UserId);
        }

        [HttpPut]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void CreateItem(NewListItem item)
        {
            var data = _listService.CreateListItem(item.ListId, item.Name, item.Count);
            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemCreatedMessage(data.Item, _sessionContext.Username), data.List.Users);
            }

            _bus.Publish(new ItemCreatedMessage(data.Item), data.List.UserId);
        }

    }
}