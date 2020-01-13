namespace Adapter
{
    public static class Client
    {
        static Adaptee adaptee = new Adaptee();

        public static void Run()
        {
            Adaptee adaptee = new Adaptee();
            ITarget target = new Adapter(adaptee);

            target.GetRequest();
        }
    }

    // The Adaptee contains some useful behavior, but its interface is
    // incompatible with the existing client code. The Adaptee needs some
    // adaptation before the client code can use it.
    class Adaptee
    {
        public void DoSomething()
        {
            //... 3rd party or legacy stuff ...
        }
        public string GetSpecificRequest() => "Specific request.";
    }

    // The Target defines the domain-specific interface used by the client code.
    public interface ITarget
    {
        string GetRequest();
    }

    // The Adapter makes the Adaptee's interface compatible with the Target's
    // interface.
    class Adapter : ITarget
    {
        readonly Adaptee adaptee;
        public Adapter(Adaptee adaptee) => this.adaptee = adaptee;
        public string GetRequest() => $"This is '{adaptee.GetSpecificRequest()}'";
    }
}