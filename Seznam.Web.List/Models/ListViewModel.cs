using System.Collections.Generic;

namespace Seznam.Web.List.Models
{
    public class ListViewModel
    {
        public string Name { get; set; }
        public IEnumerable<SeznamList> Lists { get; set; }
        public string Id { get; set; }
    }
}