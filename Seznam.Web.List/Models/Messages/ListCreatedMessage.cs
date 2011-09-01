using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class ListCreatedMessage
    {
        public ListCreatedMessage(Data.Services.List.Contracts.SeznamList list)
        {
            List = list;
        }

        public string EventType { get { return "listCreated"; } }
        public Data.Services.List.Contracts.SeznamList List { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(ListCreatedMessage m)
        {
            return m.ToString();
        }
    }
}