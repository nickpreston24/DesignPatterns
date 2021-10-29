using System;
using System.Collections.Generic;

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
    
    
    public static class Client
    {
        // The client code is usually suited to work with a single handler. In
        // most cases, it is not even aware that the handler is part of a chain.
        public static void ClientCode(AbstractHandler handler)
        {
            foreach (var food in new List<string> { "Nut", "Banana", "Cup of coffee" })
            {
                Console.WriteLine($"Client: Who wants a {food}?");

                var result = handler.Handle(food);

                if (result != null)
                {
                    Console.Write($"   {result}");
                }
                else
                {
                    Console.WriteLine($"   {food} was left untouched.");
                }
            }
        }
    }
}