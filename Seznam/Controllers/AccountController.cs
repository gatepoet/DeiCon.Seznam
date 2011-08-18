using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Seznam.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

    public class BaseController : Controller
    {
        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            base.Execute(requestContext);
        }
    }

    public class SignupResponse
    {
        public static SignupResponse Error(string message)
        {
            return new SignupResponse
                       {
                           Ok = false,
                           Message = message
                       };

        }
        public static SignupResponse Success(string userId)
        {
            return new SignupResponse
                       {
                           Ok = true,
                           UserId = userId
                       };

        }

        public bool Ok { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
    public class SimpleResponse
    {
        public static SignupResponse Error(string message)
        {
            return new SignupResponse
                       {
                           Ok = false,
                           Message = message
                       };

        }
        public static SignupResponse Success()
        {
            return new SignupResponse
                       {
                           Ok = true,
                       };

        }

        public bool Ok { get; set; }
        public string Message { get; set; }
    }

    public static class JsonNetExtensions
    {
        public static JsonNetResult ToJsonResult(this object data)
        {
            return new JsonNetResult
                       {
                           Formatting = Formatting.Indented,
                           Data = data,
                           SerializerSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}
                       };
        }
    }


    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
              ? ContentType
              : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
                
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Data, Formatting, SerializerSettings);
        }
    }
}
