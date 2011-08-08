using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Seznam.Models;
using Seznam.Utilities;
using System.Web.Helpers;

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
            //var user = _userRepository.GetUser(_sessionContext.Username);

            
            //var viewModel = new ListHomeViewModel
            //                    {
            //                        PersonalListCount = user.PersonalLists.Count(),
            //                        SharedListCount = user.SharedLists.Count()
            //                    };
            //return View(viewModel);
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
        public JsonNetResult All()
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            
            return user.ToJsonResult();
        }

        public void test()
        {
            var user = new User("k", "k");
            _userRepository.Add(user);
            var json = new
            {
                personalLists = user.PersonalLists.Select(l => new
                {
                    name = l.Name,
                    count = l.Count

                }).ToArray(),
                sharedLists = user.SharedLists.Select(l => new
                {
                    name = l.Name,

                }).ToArray(),
            };
            Console.WriteLine(user.ToJsonResult().ToString());
        }

        [HttpPut]
        public JsonNetResult Create(NewList data)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            var list = user.CreateNewList(data.Name, data.Shared, data.Users);

            return new { ok = true, id = list.Id.ToString() }.ToJsonResult();
        }

        [HttpPut]
        public JsonResult AddPersonalItem(string listName, string name)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            user.GetPersonalList(listName).CreateNewItem(name);

            return Json(new { ok = true });
        }
    }

    public class NewList
    {
        public string Name { get; set; }

        public bool Shared { get; set; }

        public string[] Users { get; set; }
    }
}
