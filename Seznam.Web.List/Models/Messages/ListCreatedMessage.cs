using Seznam.List.Contracts;
using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class ListCreatedMessage
    {
        public ListCreatedMessage(SeznamList list)
        {
            List = list;
        }

        public string EventType { get { return "listCreated"; } }
        public SeznamList List { get; set; }
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