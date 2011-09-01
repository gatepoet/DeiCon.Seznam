using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Seznam.Web.List.Models
{
    [Serializable]
    public class SeznamList : IEnumerable<SeznamListItem>
    {
        private readonly Dictionary<string, SeznamListItem> _list = new Dictionary<string, SeznamListItem>();
        public string[] Users { get; set; }

        public SeznamList(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public SeznamList()
        {
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Shared { get; set; }

        public int Count { get { return _list.Count; } }

        public void Add(SeznamListItem item)
        {
            _list.Add(item.Name, item);
        }
        public void Remove(SeznamListItem item)
        {
            _list.Remove(item.Name);
        }

        public IEnumerable<SeznamListItem> Items { get { return _list.Values; } }

        public IEnumerator<SeznamListItem> GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public SeznamListItem GetItem(string name)
        {
            return _list[name];
        }

        public SeznamListItem CreateNewItem(string name, int count)
        {
            var listItem = new SeznamListItem(name, count);
            _list.Add(name, listItem);
            return listItem;
        }
    }
}