using Seznam.Web.Common;

namespace Seznam.Web.Account.Models
{
    public class SignupResponse
    {
        public static JsonNetResult Error(string message)
        {
            return new SignupResponse
                       {
                           Ok = false,
                           ErrorMessage = message
                       }.ToJsonResult();

        }
        public static JsonNetResult Success(string userId, string username)
        {
            return new SignupResponse
                       {
                           Ok = true,
                           Message =new SignedUpMessage
                               {
                                   UserId = userId,
                                   Username = username
                               }
                       }.ToJsonResult();

        }

        public bool Ok { get; set; }
        public SignedUpMessage Message { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SignedUpMessage
    {
        public string UserId { get; set; }

        public string Username { get; set; }
    }
}