using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;
using Seznam.Data;
using Seznam.Data.Services.User;
using Seznam.Data.Services.User.Contracts;
using Seznam.Models;
using Seznam.Utilities;
using User = Seznam.Models.User;


namespace Seznam.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ISessionContext _sessionContext;

        public AccountController()
        {
            _userService = new UserService();
            _sessionContext = SessionContext.Current;
        }

        public AccountController(IUserService userService, ISessionContext sessionContext)
        {
            _userService = userService;
            _sessionContext = sessionContext;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonNetResult Login(LoginViewModel viewModel)
        {
            try
            {
                var username = viewModel.Username;
                var userId = _userService.Authenticate(username, viewModel.Password);

                SetUser(userId, username);

                return SignupResponse.Success(username).ToJsonResult();
            }
            catch (AuthenticationException exception)
            {
                return SignupResponse.Error(exception.Message).ToJsonResult();
            }

        }

        private void SetUser(string userId, string username)
        {
            _sessionContext.Username = username;
            _sessionContext.UserId = userId;
            FormsAuthentication.SetAuthCookie(userId, false);
        }

        [HttpPost]
        public JsonNetResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return SimpleResponse.Success().ToJsonResult();
        }

        [HttpGet]
        public ViewResult Signup()
        {
            return View();
        }

        [HttpPut]
        public JsonNetResult SignUp(SignupViewModel viewModel)
        {
            var username = viewModel.Username;

            try
            {
                var userId = _userService.CreateUser(viewModel.Username, viewModel.Password);
                SetUser(userId, username);
                return SignupResponse.Success(username).ToJsonResult();
            }
            catch (UserExistsException exception)
            {
                return SignupResponse.Error("Username is taken. Try again!").ToJsonResult();
            }
        }

        public ActionResult LoggedOut()
        {
            return View();
        }
    }
}
