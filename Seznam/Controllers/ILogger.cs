using System;

namespace Seznam.Controllers
{
    public interface ILogger
    {
        void Error(Exception ex);
        void Info(string message);
        void Warn(string message);
        void Warn(Exception ex);
        void Debug(string message);
    }
}