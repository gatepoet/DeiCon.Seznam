using System.Collections.Generic;

namespace Seznam.List.Contracts
{
    public class SeznamSummmary
    {
        public List<SeznamList> PersonalLists { get; set; }
        public List<SeznamList> SharedLists { get; set; }
    }
}