using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class ItemCreatedMessage
    {
        public ItemCreatedMessage(Data.Services.List.Contracts.SeznamListItem item)
        {
            Item = item;
        }

        public string EventType { get { return "itemCreated"; } }
        public Data.Services.List.Contracts.SeznamListItem Item { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ItemCreatedMessage m)
        {
            return m.ToString();
        }
    }
}