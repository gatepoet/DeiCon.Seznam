using System.Collections.Generic;
using FM.WebSync.Core;

namespace Seznam.Controllers
{
    public class Bus
    {
        private readonly string _url;

        public Bus(string url)
        {
            _url = url;
        }
        private Publisher CreatePublisher()
        {
            var p = new Publisher(new PublisherArgs { RequestUrl = _url });
            return p;
        }
        public void Publish(string json, string userId)
        {
            var p = CreatePublisher();
            p.Publish(string.Format("/user/{0}", userId), json);
        }
        public void Publish(string json, IEnumerable<string> userIds)
        {
            var p = CreatePublisher();
            foreach (var userId in userIds)
            {
                p.Publish(string.Format("/user/{0}", userId), json);
            }
        }

    }
}