using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Seznam.Models
{
    [Serializable]
    public class SeznamList : IEnumerable<SeznamListItem>
    {
        private readonly Dictionary<string, SeznamListItem> _list = new Dictionary<string, SeznamListItem>();

        public SeznamList(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public SeznamList()
        {
        }

        public Guid Id
        {
            get; set;
        }

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

        public void CreateNewItem(string name)
        {
            _list.Add(name, new SeznamListItem(name));
        }
    }
    
    public class test
    {
        public void t()
        {
            var o = new SeznamListItem("asdasd") {Completed = true};
            var stream = new MemoryStream();

            new XmlSerializer(typeof(SeznamListItem)).Serialize(stream, o);
            Console.WriteLine(new StreamReader(stream).ReadToEnd());


            Console.WriteLine(Json.Encode(o));
        }
    
        public void t2()
        {
            var list = new SeznamList("aiiii")
                           {
                               new SeznamListItem("iasd"),
                               new SeznamListItem("lksjdft")
                           };


            var jlist = new
            {
                name = list.Name,
                items = list,
            };
            string json =
                JsonConvert.SerializeObject(
                    jlist,
                    Formatting.None,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                    );

            Console.WriteLine(json);

            var stream = new MemoryStream();
            var reader = new StreamReader(stream);
            new JsonSerializer().Serialize(new StreamWriter(stream), jlist);
            Console.WriteLine("--");
            Console.WriteLine(reader.ReadToEnd());
        }
    
    }

    [Serializable]
    public class T1 : IEnumerable<string>
    {
        public string Name { get; set; }

        public void Add(string s){}

        private readonly List<string> _list = new List<string>();

        public List<string> Items { get { return _list; } }

        public IEnumerator<string> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}