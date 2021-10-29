using System;
using System.Diagnostics;
using DesignPatterns.Singletons;

namespace DP.Tests.Singletons.Classes
{
    public class ClientLogger : ILogger
    {
        public string Provider { get; set; } = nameof(ClientLogger);

        public static ClientLogger Instance => Singleton<ClientLogger>.Instance;

        ClientLogger()
        {
        }

        public void Log(Exception exception) => Console.WriteLine(exception.ToString());

        public void Log(string message) => Console.WriteLine(message);
    }

    public class EmailLogger : ILogger
    {
        public string Provider { get; set; } = nameof(EmailLogger);

        public static EmailLogger Instance => Singleton<EmailLogger>.Instance;

        EmailLogger()
        {
        }

        public void Log(Exception exception) => Debug.WriteLine(exception.ToString());

        public void Log(string message) => Debug.WriteLine(message);
    }
}