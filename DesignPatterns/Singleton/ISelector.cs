namespace DesignPatterns
{
    public interface ISelector
    {
        ISingleton GetInstance<T>() where T : class, ISingleton;
    }
}
