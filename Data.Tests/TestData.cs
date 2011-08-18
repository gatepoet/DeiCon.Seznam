using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using Raven.Client;
using Raven.Database.Config;

namespace Seznam.Data.Tests
{
    public class TestData : IEntity
    {
        public bool Equals(TestData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id) && other.Count == Count && Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TestData)) return false;
            return Equals((TestData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Id != null ? Id.GetHashCode() : 0);
                result = (result*397) ^ Count;
                result = (result*397) ^ (Name != null ? Name.GetHashCode() : 0);
                return result;
            }
        }

        public string Id { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }
    }
}
