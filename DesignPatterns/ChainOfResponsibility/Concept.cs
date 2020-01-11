namespace ChainOfResponsibility.Conceptual
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        object Handle(object request);
    }

    public abstract class AbstractHandler : IHandler
    {
        IHandler nextHandler;
        public IHandler SetNext(IHandler handler)
        {
            nextHandler = handler;
            return handler;
        }

        public virtual object Handle(object request) => nextHandler?.Handle(request);
    }

    public class MonkeyHandler : AbstractHandler
    {
        public override object Handle(object request) =>
            (request as string) == "Banana" 
                ? $"Monkey: I'll eat the {request}.\n" 
                : base.Handle(request);
    }

    public class SquirrelHandler : AbstractHandler
    {
        public override object Handle(object request) =>
            request.ToString() == "Nut"
                ? $"Squirrel: I'll eat the {request}.\n"
                : base.Handle(request);
    }

    public class DogHandler : AbstractHandler
    {
        public override object Handle(object request) =>
            request.ToString() == "MeatBall"
                ? $"Dog: I'll eat the {request}.\n"
                : base.Handle(request);
    } 
}