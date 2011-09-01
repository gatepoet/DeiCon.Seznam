using System;
using System.Linq;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;
using Seznam.Data.Services.User;
using Seznam.Data.Services.User.Contracts;
using Seznam.Web.Account.Models;
using Seznam.Web.Common;

namespace Seznam.Web.Account.Controllers
{
    public class AccountController : Controller
    {
        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            base.Dispose(disposing);
        }
        private readonly IUserService _userService;
        
        public AccountController()
        {
            _userService = new UserService();
        }

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet] public ViewResult Login()     { return View(); }
        [HttpGet] public ViewResult LoggedOut() { return View(); }
        [HttpGet] public ViewResult Signup()    { return View(); }


        [HttpPost]
        public JsonNetResult Login(LoginViewModel viewModel)
        {
            try
            {
                var username = viewModel.Username.Trim();
                var userId = _userService.Authenticate(username, viewModel.Password.Trim());

                SetUser(userId, username);

                return SignupResponse.Success(userId, username);
            }
            catch (AuthenticationException exception)
            {
                return SignupResponse.Error(exception.Message);
            }

        }

        private void SetUser(string userId, string username)
        {
            var session = ControllerContext.HttpContext.Session;
            session["Username"] = username;
            session["UserId"] = userId;

            FormsAuthentication.SetAuthCookie(userId, false);
        }

        [HttpPost]
        public JsonNetResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return SimpleResponse.Success().ToJsonResult();
        }


        [HttpPut]
        public JsonNetResult SignUp(SignupViewModel viewModel)
        {
            var username = viewModel.Username.Trim();

            try
            {
                var userId = _userService.CreateUser(username, viewModel.Password.Trim());
                SetUser(userId, username);
                return SignupResponse.Success(userId, username).ToJsonResult();
            }
            catch (UserExistsException exception)
            {
                return SignupResponse.Error("Username is taken. Try again!").ToJsonResult();
            }
        }

        [HttpGet]
        public JsonNetResult Ids(string usernames)
        {
            var names = usernames
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(u => u.Trim());
            var ids = _userService.GetUserIds(names);

            return DataResponse.Success(ids);
        }
    }
}
