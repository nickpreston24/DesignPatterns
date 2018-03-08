using System;

namespace DesignPatterns.Tests
{
    public interface ILogger : ISingleton
    {
        string Name { get; set; }
        void Log(Exception exception);
        void Log(string message);
    }
}