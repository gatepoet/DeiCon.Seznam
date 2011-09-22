using System.Collections.Generic;
using FM.WebSync.Core;
using FM.WebSync.Server;

namespace Seznam.Web.List.Controllers
{
    public class Bus
    {
        private readonly string _url;

        public Bus(string url)
        {
            _url = url;
        }

        public void Publish(string json, string userId)
        {
            RequestHandler.Publish(string.Format("/user/{0}", userId), json);
        }
        public void Publish(string json, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                RequestHandler.Publish(string.Format("/user/{0}", userId), json);
            }
        }

    }
}