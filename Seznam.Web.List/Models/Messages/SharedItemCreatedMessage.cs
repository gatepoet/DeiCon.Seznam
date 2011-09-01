using Seznam.List.Contracts;
using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class SharedItemCreatedMessage
    {
        public SharedItemCreatedMessage(SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemCreated"; } }
        public SeznamListItem Item { get; set; }
        public string Username { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedItemCreatedMessage m)
        {
            return m.ToString();
        }
    }
}