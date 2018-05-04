using System;
using System.Diagnostics;

namespace DesignPatterns.Tests
{
    public class ClientLogger : ILogger
    {
        public string Name { get; set; } = nameof(ClientLogger);
        private ClientLogger() { }

        public static ClientLogger Instance
        {
            get
            {
                return Singleton<ClientLogger>.Instance;
            }
        }

        public void Log(Exception exception) => Debug.WriteLine(exception.ToString());

        public void Log(string message) => Debug.WriteLine(message);
    }

    public class EmailLogger : ILogger
    {
        private EmailLogger() { }
        public string Name { get; set; } = nameof(EmailLogger);

        public void Log(Exception exception) => Debug.WriteLine(exception.ToString());

        public void Log(string message) => Debug.WriteLine(message);
    }
}