namespace DesignPatterns.Tests
{
    public interface ILogger : ISingleton
    {
        string Name { get; set; }
        void Log(string message);
    }
}