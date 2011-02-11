using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        [HttpGet]
        public JsonResult Detail(string name)
        {
            var user = _userRepository.GetUser(_sessionContext.Username);
            var list = user.GetPersonalList(name);
            var json = new
                           {
                               name = list.Name,
                               count = list.Count,
                               totalCount = user.PersonalLists.Count()
                           };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
