using System;

namespace Seznam.Web.Common
{
    public class NullLogger : ILogger
    {
        public void Error(Exception ex)
        {
        }

        public void Info(string message)
        {
        }

        public void Warn(string message)
        {
        }

        public void Warn(Exception ex)
        {
        }

        public void Debug(string message)
        {
        }
    }
}