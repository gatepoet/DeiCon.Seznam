using System;
using System.Runtime.Serialization;

namespace Seznam.Data
{
    public class ListItemExistsException : Exception
    {
        public ListItemExistsException()
        {
        }

        public ListItemExistsException(string message) : base(message)
        {
        }

        public ListItemExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ListItemExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}