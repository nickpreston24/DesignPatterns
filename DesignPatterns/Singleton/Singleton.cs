using System;
using System.Reflection;

namespace DesignPatterns.Singletons
{
    //original source: http://richardpianka.com/2011/01/generic-singleton-pattern-in-c-with-reflection/

    /// Factory    
    public static class Singleton<T> where T : class, ISingleton
    {
        static volatile T instance;
        static readonly object @lock = new object();
        const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;

        static Singleton() { }

        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (@lock)
                {
                    if (instance == null)
                    {
                        ConstructorInfo constructor = null;

                        try
                        {
                            // Binding flags exclude public constructors.
                            constructor = typeof(T).GetConstructor(FLAGS, null, new Type[0], null);
                        }
                        catch (Exception exception)
                        {
                            throw new SingletonException(exception);
                        }

                        if (constructor == null || constructor.IsAssembly)
                        {
                            // Also exclude internal constructors.
                            throw new SingletonException(string.Format("A private or " +
                                  "protected empty parameterless constructor is missing for '{0}'.", typeof(T).Name));
                        }

                        instance = (T)constructor.Invoke(null);
                        //instance.Selector = new Selector();
                    }
                }

                return instance;
            }
        }

        #region For Use in C# 8
        //Todo: Replace with Selector's implementation when C# 8.0 comes out.
        /////Wrapper-Instance (Holds Implemenation)
        //class Selector : ISelector
        //{
        //    ISelector Implementation { get; set; }

        //    public Selector()
        //    {
        //        Implementation = new SelectorImplementation();
        //    }

        //    public ISingleton GetInstance<T>() where T : class, ISingleton
        //    {
        //        return Implementation.GetInstance<T>();
        //    }
        //}
        #endregion For Use in C# 8
    }

    ///Implements
    internal class SelectorImplementation : ISelector
    {
        public virtual ISingleton GetInstance<T>() where T : class, ISingleton => Multiton.GetInstance<T>();
    }

    public static class SingletonExtensions
    {
        public static ISingleton Get<T>(this ISingleton singleton) where T : class, ISingleton => Multiton.GetInstance<T>();
    }
}
