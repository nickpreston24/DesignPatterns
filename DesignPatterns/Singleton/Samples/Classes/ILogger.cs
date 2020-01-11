namespace DesignPatterns.Tests
{
    public interface ILogger
    {
        string Name { get; set; }
        void Log(string message);
    }
}