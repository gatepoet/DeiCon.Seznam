using Seznam.List.Contracts;
using Seznam.Web.Common;

namespace Seznam.Web.List.Models.Messages
{
    public class SharedListCreatedMessage
    {
        public SharedListCreatedMessage(SeznamList list, string userName)
        {
            List = list;
            Username = userName;
        }

        public string EventType { get { return "sharedListCreated"; } }
        public SeznamList List { get; set; }
        public string Username { get; set; }
        public override string ToString()
        {
            return this.ToJsonString();
        }
        public static implicit operator string(SharedListCreatedMessage m)
        {
            return m.ToString();
        }
    }
}