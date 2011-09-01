namespace Seznam.List.Contracts
{
    public class SeznamListItem
    {
        public SeznamListItem()
        {
        }

        public SeznamListItem(string listId, string name, int count)
        {
            ListId = listId;
            Name = name;
            Count = count;
        }

        public static bool operator ==(SeznamListItem left, SeznamListItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SeznamListItem left, SeznamListItem right)
        {
            return !Equals(left, right);
        }

        public bool Equals(SeznamListItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ListId, ListId) && Equals(other.Name, Name) && other.Count == Count && other.Completed.Equals(Completed);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SeznamListItem)) return false;
            return Equals((SeznamListItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (ListId != null ? ListId.GetHashCode() : 0);
                result = (result*397) ^ (Name != null ? Name.GetHashCode() : 0);
                result = (result*397) ^ Count;
                result = (result*397) ^ Completed.GetHashCode();
                return result;
            }
        }

        public string ListId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Completed { get; set; }
    }
}