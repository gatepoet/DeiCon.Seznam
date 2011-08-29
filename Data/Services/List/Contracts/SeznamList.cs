using System.Collections.Generic;
using System.Linq;

namespace Seznam.Data.Services.List.Contracts
{
    public class SeznamList
    {
        public SeznamList() { }
        public SeznamList(string userId, string name, bool shared, string[] users)
        {
            UserId = userId;
            Name = name;
            Shared = shared;
            Users = new List<string>(users ?? new string[0]);
            Items = new List<SeznamListItem>();
        }
        public SeznamList(string userId, string name, bool shared, string users = "")
        {
            UserId = userId;
            Name = name;
            Shared = shared;
            Users = new List<string>();
            foreach (var user in users.Split(',').Select(u => u.Trim()).Where(user => user != string.Empty))
            {
                Users.Add(user);
            }
            Items = new List<SeznamListItem>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<SeznamListItem> Items { get; set; }
        public bool Shared { get; set; }
        public List<string> Users { get; set; }

        public string UserId { get; set; }

        public static bool operator ==(SeznamList left, SeznamList right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SeznamList left, SeznamList right)
        {
            return !Equals(left, right);
        }

        public bool Equals(SeznamList other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id) && Equals(other.Name, Name) && other.Items.SequenceEqual(Items) && other.Shared.Equals(Shared) && other.Users.SequenceEqual(Users) && Equals(other.UserId, UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SeznamList)) return false;
            return Equals((SeznamList) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Id != null ? Id.GetHashCode() : 0);
                result = (result*397) ^ (Name != null ? Name.GetHashCode() : 0);
                result = (result*397) ^ (Items != null ? Items.GetHashCode() : 0);
                result = (result*397) ^ Shared.GetHashCode();
                result = (result*397) ^ (Users != null ? Users.GetHashCode() : 0);
                result = (result*397) ^ (UserId != null ? UserId.GetHashCode() : 0);
                return result;
            }
        }

        public SeznamListItem AddItem(string name, int count)
        {
            if (Items.Any(i => i.Name == name))
                throw new ListItemExistsException(string.Format("Item with name '{0}' already exists in list '{1}'.", name, Name));

            var item = new SeznamListItem(Id, name, count);
            Items.Add(item);
            return item;
        }
    }
}