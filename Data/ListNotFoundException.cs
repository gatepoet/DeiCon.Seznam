using System;
using System.Runtime.Serialization;

namespace Seznam.Data
{
    public class ListNotFoundException : Exception
    {
        public ListNotFoundException()
        {
        }

        public ListNotFoundException(string message) : base(message)
        {
        }

        public ListNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}