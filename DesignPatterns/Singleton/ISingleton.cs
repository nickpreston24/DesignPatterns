namespace DesignPatterns
{
    public interface ISingleton
    {
    }

    public interface ISingleton<T> : ISingleton where T : class
    {
        Selector<T> Selector { get; set; }
    }
}