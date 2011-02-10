using System.Collections.Generic;
using Seznam.Controllers;

namespace Seznam.Models
{
    public class ListViewModel
    {
        public string Name { get; set; }
        public IEnumerable<SeznamList> Lists { get; set; }
        public string Id { get; set; }
    }
}