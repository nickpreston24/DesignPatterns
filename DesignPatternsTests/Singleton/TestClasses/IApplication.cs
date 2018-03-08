namespace DesignPatterns.Tests
{
    interface IApplication : ISingleton
    {
        string Name { get; }
        string Version { get; }
    }
}