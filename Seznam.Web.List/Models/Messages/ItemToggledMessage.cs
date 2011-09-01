using Seznam.List.Contracts;
using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class ItemToggledMessage
    {
        public ItemToggledMessage(SeznamListItem item, string name)
        {
            Item = item;
        }

        public string EventType { get { return "itemToggled"; } }
        public string Username { get; set; }
        public SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ItemToggledMessage m)
        {
            return m.ToString();
        }
    }
}