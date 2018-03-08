using System;
using System.Diagnostics;

namespace DesignPatterns.Tests
{
    public class Logger : ILogger
    {
        public string Name { get; set; }
        private Logger() { }

        public static Logger Instance
        {
            get
            {
                return Singleton<Logger>.Instance;
            }
        }

        public void Log(Exception ex) => Debug.WriteLine(ex.ToString());

        public void Log(string message) => Debug.WriteLine(message);
    }
}