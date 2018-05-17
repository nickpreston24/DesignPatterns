using System;
using System.Reflection;

namespace DesignPatterns.Tests
{
    ///From: https://www.codeproject.com/Articles/630324/Dependency-Injected-Singletons-What
    ///https://effectivesoftwaredesign.com/2015/06/11/the-solid-principles-illustrated-by-design-patterns/
    ///https://stackoverflow.com/questions/18488522/how-to-use-interface-with-singleton-class
    public class Application : IApplication
    {
        private Application() { }  //permitted.
        public static Application Instance => Singleton<Application>.Instance; //use this with generic singleton
        public string Name { get; } = "Notepad++";
        public string Version { get; } = "1.0.0";
        public ISelector Selector { get; set; }

        public static event EventHandler<QueryTypeEventArgs> QueryType;

        //use to make Interface singletons
        private static IApplication CreateInstance()
        {
            var handler = QueryType;
            if (handler == null)
            {
                throw new InvalidOperationException(
                    "Cannot create an instance because the QueryType event " +
                    "handler was never set.");
            }

            var args = new QueryTypeEventArgs();
            handler.Invoke(null, args);

            if (args.Type == null)
            {
                throw new InvalidOperationException(
                    "Cannot create an instance because the type has not been " +
                    "provided.");
            }

            if (!typeof(IApplication).IsAssignableFrom(args.Type))
            {
                throw new InvalidOperationException(
                    "Cannot create an instance because the provided type does " +
                    "not implement the IApplication interface.");
            }

            const BindingFlags FLAGS =
                BindingFlags.CreateInstance |
                BindingFlags.Instance |
                BindingFlags.NonPublic;

            var constructors = args.Type.GetConstructors(FLAGS);

            if (constructors.Length != 1)
            {
                throw new InvalidOperationException(
                    "Cannot create an instance because a single private " +
                    "parameterless constructor was expected.");
            }

            return (IApplication)constructors[0].Invoke(null);
        }
    }
}