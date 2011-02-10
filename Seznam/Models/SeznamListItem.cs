namespace Seznam.Models
{
    public class SeznamListItem
    {
        public SeznamListItem(string name)
        {
            Name = name;
        }
 
        public string Name { get; private set; }
        public void ChangeName(string name)
        {
            Name = name;
        }

        public int Count { get; private set; }
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

        public bool Completed { get; private set; }
        public void Complete()
        {
            Completed = true;
        }
    }
}