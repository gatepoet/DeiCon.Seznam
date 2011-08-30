using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Seznam.Data;
using Seznam.Data.Services.List;
using Seznam.Data.Services.List.Contracts;
using Seznam.Data.Services.User;
using Seznam.Data.Services.User.Contracts;
using Seznam.Models;
using Seznam.Utilities;
using System.Web.Helpers;
using SeznamList = Seznam.Data.Services.List.Contracts.SeznamList;
using SeznamListItem = Seznam.Data.Services.List.Contracts.SeznamListItem;
using User = Seznam.Models.User;

namespace Seznam.Controllers
{
    public class ListController : Controller
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

        public ListController()
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
        public ListController(IListService listService, ISessionContext sessionContext, ILogger logger)
        {
            _listService = listService;
            _sessionContext = sessionContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Index2()
        {
            return View();
        }

        [HttpGet]
        public ViewResult My()
        {
            return View("List");
        }

        [HttpGet]
        public ViewResult Shared()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult All()
        {
            var user = _listService.GetSummary(_sessionContext.UserId);
            
            return DataResponse.Success(user);
        }

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
        public void CreatePersonalListItem(NewListItem item)
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
        public void DeletePersonalListItem(DeletePersonalListItemMessage message)
        {
            var data  = _listService.DeleteItem(message.ListId, message.Name);

            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemDeletedMessage(data.Item, _sessionContext.Username), data.List.Users);
            }
            _bus.Publish(new ItemDeletedMessage(data.Item, _sessionContext.Username), data.List.UserId);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult UpdatePersonalListItem(NewListItem data)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void TogglePersonal(ToggleData td)
        {
            var data = _listService.TogglePersonalItem(td.ListId, td.ItemName, td.ItemCompleted);

            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemToggledMessage(data.Item, _sessionContext.Username), data.List.Users);
            }
            _bus.Publish(new ItemToggledMessage(data.Item, _sessionContext.Username), data.List.UserId);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public void ToggleShared(ToggleData td)
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
        public void CreateSharedItem(NewListItem item)
        {
            var data = _listService.CreateListItem(item.ListId, item.Name, item.Count);
            if (data.List.Shared)
            {
                _bus.Publish(new SharedItemCreatedMessage(data.Item, _sessionContext.Username), data.List.Users);
            }

            _bus.Publish(new ItemCreatedMessage(data.Item), data.List.UserId);
        }

    }

    public class ItemCreatedMessage
    {
        public ItemCreatedMessage(SeznamListItem item)
        {
            Item = item;
        }

        public string EventType { get { return "itemCreated"; } }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ItemCreatedMessage m)
        {
            return m.ToString();
        }
    }
    public class SharedItemCreatedMessage
    {
        public SharedItemCreatedMessage(SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemCreated"; } }
        public SeznamListItem Item { get; set; }
        public string Username { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedItemCreatedMessage m)
        {
            return m.ToString();
        }
    }
    public class ListCreatedMessage
    {
        public ListCreatedMessage(SeznamList list)
        {
            List = list;
        }

        public string EventType { get { return "listCreated"; } }
        public SeznamList List { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ListCreatedMessage m)
        {
            return m.ToString();
        }
    }

    public class SharedListCreatedMessage
    {
        public SharedListCreatedMessage(SeznamList list, string userName)
        {
            List = list;
            Username = userName;
        }

        public string EventType { get { return "sharedListCreated"; } }
        public SeznamList List { get; set; }
        public string Username { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedListCreatedMessage m)
        {
            return m.ToString();
        }
    }

    public class SharedItemToggledMessage
    {
        public SharedItemToggledMessage(SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemToggled"; } }
        public string Username { get; set; }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedItemToggledMessage m)
        {
            return m.ToString();
        }
    }
    public class ItemToggledMessage
    {
        public ItemToggledMessage(SeznamListItem item, string name)
        {
            Item = item;
        }

        public string EventType { get { return "itemToggled"; } }
        public string Username { get; set; }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ItemToggledMessage m)
        {
            return m.ToString();
        }
    }
    public class SharedItemDeletedMessage
    {
        public SharedItemDeletedMessage(SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemDeleted"; } }
        public string Username { get; set; }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedItemDeletedMessage m)
        {
            return m.ToString();
        }
    }
    public class ItemDeletedMessage
    {
        public ItemDeletedMessage(SeznamListItem item, string name)
        {
            Item = item;
        }

        public string EventType { get { return "itemDeleted"; } }
        public string Username { get; set; }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ItemDeletedMessage m)
        {
            return m.ToString();
        }
    }
}
