using System;
using System.Web.Mvc;
using Seznam.Data.Services.List;
using Seznam.Data.Services.List.Contracts;
using Seznam.Web.Common;
using Seznam.Web.List.Models;
using Seznam.Web.List.Models.Messages;
using SeznamList = Seznam.Data.Services.List.Contracts.SeznamList;

namespace Seznam.Web.List.Controllers
{
    public class PersonalController : Controller
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

        public PersonalController()
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
        public PersonalController(IListService listService, ISessionContext sessionContext, ILogger logger)
        {
            _listService = listService;
            _sessionContext = sessionContext;
            _logger = logger;
        }
        [HttpGet] public ViewResult Index() { return View(); }
        [HttpGet] public ViewResult Detail() { return View(); }
        [HttpGet] public ViewResult Create() { return View(); }

        [HttpPut]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void Create(NewList data)
        {
            var list = _listService.CreateList(new SeznamList(_sessionContext.UserId, data.Name, data.Shared, data.Users));
            if (list.Shared)
            {
                _bus.Publish(new SharedListCreatedMessage(list, _sessionContext.Username), list.Users);
            }

            _bus.Publish(new ListCreatedMessage(list), _sessionContext.UserId);
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

            _bus.Publish(new ItemCreatedMessage(data.Item), _sessionContext.UserId);
        }

        [HttpDelete]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void DeleteItem(DeletePersonalListItemMessage message)
        {
            var data = _listService.DeleteItem(message.ListId, message.Name);

            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemDeletedMessage(data.Item, _sessionContext.Username), data.List.Users);
            }
            _bus.Publish(new ItemDeletedMessage(data.Item, _sessionContext.Username), data.List.UserId);
        }


        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void Toggle(ToggleData td)
        {
            var data = _listService.TogglePersonalItem(td.ListId, td.ItemName, td.ItemCompleted);

            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemToggledMessage(data.Item, _sessionContext.Username), data.List.Users);
            }
            _bus.Publish(new ItemToggledMessage(data.Item, _sessionContext.Username), data.List.UserId);
        }

    }
}
