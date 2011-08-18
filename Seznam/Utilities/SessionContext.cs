using System.Web;

namespace Seznam.Utilities
{
    public class SessionContext : ISessionContext
    {
        public string Username
        {
            get { return (string) HttpContext.Current.Session["Username"]; }
            set { HttpContext.Current.Session["Username"] = value; }
        }
        public string UserId
        {
            get { return (string) HttpContext.Current.Session["UserId"]; }
            set { HttpContext.Current.Session["UserId"] = value; }
        }
        public static ISessionContext Current = new SessionContext();
    }
}