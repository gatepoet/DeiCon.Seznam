using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class SharedItemDeletedMessage
    {
        public SharedItemDeletedMessage(Data.Services.List.Contracts.SeznamListItem item, string username)
        {
            Item = item;
            Username = username;
        }

        public string EventType { get { return "sharedItemDeleted"; } }
        public string Username { get; set; }
        public Data.Services.List.Contracts.SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedItemDeletedMessage m)
        {
            return m.ToString();
        }
    }
}