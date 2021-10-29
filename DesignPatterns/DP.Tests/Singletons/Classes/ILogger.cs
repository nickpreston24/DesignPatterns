﻿ using DesignPatterns.Singletons;

  namespace DP.Tests.Singletons.Classes
{
    public interface ILogger : ISingleton
    {
        string Provider { get; set; }
        void Log(string message);
    }
}