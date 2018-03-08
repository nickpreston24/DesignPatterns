using System;
using System.Reflection;

namespace DesignPatterns
{
    //source: http://richardpianka.com/2011/01/generic-singleton-pattern-in-c-with-reflection/
    public static class Singleton<T>
       where T : class, ISingleton
    {
        static volatile T _instance;
        static object _lock = new object();
        const BindingFlags FLAGS = BindingFlags.Instance |
                      BindingFlags.NonPublic;

        static Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (_lock)
                {
                    if (_instance == null)
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
                            // Also exclude internal constructors.
                            throw new SingletonException(string.Format("A private or " +
                                  "protected empty parameterless constructor is missing for '{0}'.", typeof(T).Name));

                        _instance = (T)constructor.Invoke(null);
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// Singleton Interfaces
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="I"></typeparam>
    public abstract class Singleton<T, I>
       where T : class, ISingleton, I
    {
        //goals: from T, create the type for querytype
        //cast instance as the interface 'I'.

        static volatile T _instance;
        static object _lock = new object();
        public static event EventHandler<QueryTypeEventArgs> QueryType;

        //static Singleton()
        //{
        //}

        //public static I Instance
        //{
        //    get
        //    {
        //        if (_instance != null)
        //        {
        //            return _instance;
        //        }

        //        lock (_lock)
        //        {
        //            if (_instance == null)
        //            {
        //                ConstructorInfo constructor = null;

        //                try
        //                {
        //                    const BindingFlags FLAGS = BindingFlags.Instance |
        //                                  BindingFlags.NonPublic;
        //                    // Binding flags exclude public constructors.
        //                    constructor = typeof(T).GetConstructor(FLAGS, null, new Type[0], null);
        //                }
        //                catch (Exception exception)
        //                {
        //                    throw new SingletonException(exception);
        //                }

        //                if (constructor == null || constructor.IsAssembly)
        //                    // Also exclude internal constructors.
        //                    throw new SingletonException(string.Format("A private or " +
        //                          "protected empty parameterless constructor is missing for '{0}'.", typeof(T).Name));

        //                _instance = (T)constructor.Invoke(null);
        //            }
        //        }

        //        return _instance;
        //    }
        //}
    }
}
