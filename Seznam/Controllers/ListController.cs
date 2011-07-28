using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Seznam.Models;
using Seznam.Utilities;

namespace Seznam.Controllers
{
    public class ListController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionContext _sessionContext;

        public ListController()
        {
            _userRepository = new UserRepository();
            _sessionContext = SessionContext.Current;
        }

        public ListController(IUserRepository userRepository, ISessionContext sessionContext)
        {
            _userRepository = userRepository;
            _sessionContext = sessionContext;
        }

        //
        // GET: /List/
        [HttpGet]
        public ActionResult Index()
        {
            var user = _userRepository.GetUser(_sessionContext.Username);

            
            var viewModel = new ListHomeViewModel
                                {
                                    PersonalListCount = user.PersonalLists.Count(),
                                    SharedListCount = user.SharedLists.Count()
                                };
            return View(viewModel);
        }
        [HttpGet]
        public ViewResult Index2()
        {
            return View();
        }

        [HttpGet]
        public ViewResult My()
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            var viewModel = new ListViewModel { 
                                                Lists = user.PersonalLists,
                                };
            return View("List", viewModel);
        }

        [HttpGet]
        public JsonResult All()
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            var message = new
                              {
                                  personalLists = user.PersonalLists.Select(l => new {name=l.Name, count=l.Count}).ToArray()
                              };
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult Add(string name)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            user.CreateNewList(name);

            return Json(new { ok = true });
        }

        [HttpPut]
        public JsonResult AddPersonalItem(string listName, string name)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            user.GetPersonalList(listName).CreateNewItem(name);

            return Json(new { ok = true });
        }

        [HttpGet]
        public MyJsonResult Detail(string name)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            var list = user.GetPersonalList(name);
            var json = new
                           {
                               name = list.Name,
                               items = list,
                           };
            return new MyJsonResult(json);
            //return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PersonalItemDetail(string listName, string name)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            var item = user.GetPersonalList(listName).GetItem(name);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }

    public class MyJsonResult : ActionResult
    {
        private readonly object _data;

        public MyJsonResult(object data)
        {
            _data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(_data.ToJson());
        }
    }

    public static class JsonExtensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o,
                Formatting.None,
                new JsonSerializerSettings
                    {ContractResolver = new CamelCasePropertyNamesContractResolver()}
                );

        }
    }
}
