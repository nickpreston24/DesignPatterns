public static class Wrapper
{
    public static Wrapper<T> Create<T>(T wrapped) => new Wrapper<T>(wrapped);

    public static Wrapper<T> Wrap<T>(this T wrapped) => Create(wrapped);
}

public class Wrapper<T>
{
    public Wrapper(T wrapped) => Wrapped = wrapped;

    public T Wrapped { get; set; }
}