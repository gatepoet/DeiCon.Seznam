using System;
using System.Runtime.Serialization;

namespace Seznam.List.Contracts
{
    public class DataExistsException : Exception
    {
        public DataExistsException()
        {
        }

        public DataExistsException(string message)
            : base(message)
        {
        }

        public DataExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DataExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}