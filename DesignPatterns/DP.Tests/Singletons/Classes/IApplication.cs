using DesignPatterns.Singletons;

 namespace DP.Tests.Singletons.Classes
{
    public interface IApplication : ISingleton
    {
        string Name { get; }
        string Version { get; }
    }
}