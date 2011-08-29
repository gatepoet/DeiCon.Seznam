using System;

namespace Seznam.Controllers
{
    public class ConsoleLogger : ILogger
    {
        public void Error(Exception ex)
        {
            Console.WriteLine("ERROR");
            Console.WriteLine(ex.ToString());
        }

        public void Info(string message)
        {
            Console.WriteLine("Info");
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine("WARNING");
            Console.WriteLine(message);
        }

        public void Warn(Exception ex)
        {
            Console.WriteLine("WARNING");
            Console.WriteLine(ex.ToString());
        }

        public void Debug(string message)
        {
            Console.WriteLine("DEBUG");
            Console.WriteLine(message);
        }
    }
}