using Seznam.List.Contracts;
using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class SharedItemToggledMessage
    {
        public SharedItemToggledMessage(SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemToggled"; } }
        public string Username { get; set; }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedItemToggledMessage m)
        {
            return m.ToString();
        }
    }
}