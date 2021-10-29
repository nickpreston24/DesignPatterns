﻿ namespace DP.Tests.Singletons.Classes
{
    internal class EmailService : IEmailService, ILogger
    {
        public string Provider { get; set; }
        public ILogger Logger { get; }
        
        public EmailService(ILogger logger) => Logger = logger;

        public void Log(string message) => Logger.Log(message);
    }
}