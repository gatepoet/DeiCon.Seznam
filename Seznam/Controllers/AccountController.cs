using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Seznam.Models;
using Seznam.Utilities;


namespace Seznam.Controllers
{
    public class AccountController : Controller
    {
        private static IUserRepository _userRepository;
        private ISessionContext _sessionContext;

        public AccountController()
        {
            _userRepository = new UserRepository();
            _sessionContext = SessionContext.Current;
        }

        public AccountController(IUserRepository userRepository, ISessionContext sessionContext)
        {
            _userRepository = userRepository;
            _sessionContext = sessionContext;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public RedirectToRouteResult Login(LoginViewModel viewModel)
        {
            var username = viewModel.Username;
            
            _userRepository
                .GetUser(username)
                .Authenticate(viewModel.Password);
         
            _sessionContext.Username = username;
            FormsAuthentication.SetAuthCookie(username, viewModel.RembemberLogin);

            return RedirectToListHome();
        }
        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LoggedOut");
        }
        [HttpGet]
        public ViewResult Signup()
        {
            return View();
        }
        [HttpPost]
        public RedirectToRouteResult Signup(SignupViewModel viewModel)
        {
            var username = viewModel.Username;
            
            var user = new User(username, viewModel.Password);
            _userRepository.Add(user);
            
            _sessionContext.Username = username;
            FormsAuthentication.SetAuthCookie(username, false);
            
            return RedirectToListHome();
        }

        private RedirectToRouteResult RedirectToListHome()
        {
            return RedirectToAction("Index2", "List");
        }

        public ActionResult LoggedOut()
        {
            return View();
        }
    }
}
