using System.Collections;
using System.Collections.Generic;

namespace Seznam.Models
{
    public class SeznamList : IEnumerable<SeznamListItem>
    {
        private readonly List<SeznamListItem> _list = new List<SeznamListItem>();

        public string Name
        {
            get; set;
        }
        public int Count
        {
            get { return _list.Count; }
        }

        public void Add(SeznamListItem item)
        {
            _list.Add(item);
        }
        public void Remove(SeznamListItem item)
        {
            _list.Remove(item);
        }

        public IEnumerator<SeznamListItem> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}