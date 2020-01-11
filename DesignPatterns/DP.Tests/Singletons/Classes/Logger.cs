﻿using System;
using System.Diagnostics;

namespace DesignPatterns.Tests
{
    public class ClientLogger : ILogger
    {
        public string Name { get; set; } = nameof(ClientLogger);

        public static ClientLogger Instance
        {
            get
            {
                return Singleton<ClientLogger>.Instance;
            }
        }

        private ClientLogger()
        {
        }

        public void Log(Exception exception) => Console.WriteLine(exception.ToString());

        public void Log(string message) => Console.WriteLine(message);
    }

    public class EmailLogger : ILogger
    {
        public string Name { get; set; } = nameof(EmailLogger);

        public static EmailLogger Instance => Singleton<EmailLogger>.Instance;

        private EmailLogger()
        {
        }

        public void Log(Exception exception) => Debug.WriteLine(exception.ToString());

        public void Log(string message) => Debug.WriteLine(message);
    }
}