using System;
using System.Linq;

namespace Decorators.Conceptual
{
    internal static class Client
    {
        public static void SendOutAlerts()
        {
            var toast = Toast.Popup();
        }

        // Composes any notifier, sharing messages across all of them.
        // static ISendNotifications Compose(this ISendNotifications that, ISendNotifications other) => null;
    }

    // Base for Concrete Component
    internal abstract class Notifier : ISendNotifications
    {
        public abstract ISendNotifications Send(string message);
    }
    
    //Concrete Component
    internal class Toast : Notifier
    {
        public static Notifier Popup() => new Toast();
        public override ISendNotifications Send(string message)
        {
            Console.WriteLine($"GUI says: {message}");
            return this;
        }
    }
  

    // Base Decorator
    internal abstract class NotifierDecorator : ISendNotifications
    {
        protected  ISendNotifications wrappee;
        public NotifierDecorator(ISendNotifications notifier) => wrappee = notifier;

        /* A 'Forwarding Method':
          Implement these for all Interface methods in the Abstract base Decorator to avoid having to in Concrete Decorators.
        */
        public virtual ISendNotifications Send(string message)
        {
            wrappee.Send(message);
            return this;
        }
    }

    /*
     * Concrete Decorators
     */
    internal class SMS : NotifierDecorator
    {
        // public ISendNotifications Send(string message)
        // {
        //     Dial(4,1,7,4,8,4,3,2,3,7);
        //     Console.WriteLine("Gonna call you on your cell phone..." + message);
        //     Dial(0);
        //     return this;
        // }

        public void Dial(params int [] digits)
        {
            Console.WriteLine("Dialing...");
            digits.ToList().ForEach(Console.WriteLine);
        }

        public SMS(Notifier notifier) : base(notifier)
        {
        }
    }

    internal class Facebook : NotifierDecorator
    {
        // public ISendNotifications Send(string message)
        // {
        
        // }
        public Facebook(Notifier notifier) : base(notifier)
        {
            //     Console.WriteLine("I hope you see this post: " + message);
            //     return this;
        }
    }

    internal class Slack : NotifierDecorator
    {
        // public ISendNotifications Send(string message)
        // {
        //     Console.WriteLine("Slackbot says: " + message);
        //     return this;
        // }
        public Slack(Notifier notifier) : base(notifier)
        {
        }
    }

    // Component Interface
    internal interface ISendNotifications
    {
        ISendNotifications Send(string message);
    }
}