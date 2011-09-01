using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class SharedItemToggledMessage
    {
        public SharedItemToggledMessage(Data.Services.List.Contracts.SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemToggled"; } }
        public string Username { get; set; }
        public Data.Services.List.Contracts.SeznamListItem Item { get; set; }
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