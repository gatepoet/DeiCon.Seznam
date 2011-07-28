using System;
using System.Runtime.Serialization;

namespace Seznam.Models
{
    [Serializable]
    public class SeznamListItem
    {
        public SeznamListItem()
        {
        }

        public SeznamListItem(string name)
        {
            Name = name;
        }
 
        public string Name { get; set; }
        public void ChangeName(string name)
        {
            Name = name;
        }

        [IgnoreDataMember]
        public int Count { get; set; }
        public void SetCount(int count)
        {
            Count = count;
        }
        public void IncreaseCount()
        {
            IncreaseCount(1);
        }
        public void IncreaseCount(int amount)
        {
            Count += amount;
        }
        public void DecreaseCount()
        {
            DecreaseCount(1);
        }
        public void DecreaseCount(int amount)
        {
            Count -= amount;
        }

        public bool Completed { get; set; }
        public void Complete()
        {
            Completed = true;
        }
    }
}