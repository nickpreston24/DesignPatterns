using System;
using System.Reflection;

namespace DesignPatterns
{
    //source: http://richardpianka.com/2011/01/generic-singleton-pattern-in-c-with-reflection/
    public static class Singleton<T>
       where T : class
    {
        static volatile T _instance;
        static object _lock = new object();

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
                            constructor = typeof(T).GetConstructor(BindingFlags.Instance |
                                          BindingFlags.NonPublic, null, new Type[0], null);
                        }
                        catch (Exception exception)
                        {
                            throw new SingletonException(exception);
                        }

                        if (constructor == null || constructor.IsAssembly)
                            // Also exclude internal constructors.
                            throw new SingletonException(string.Format("A private or " +
                                  "protected constructor is missing for '{0}'.", typeof(T).Name));

                        _instance = (T)constructor.Invoke(null);
                    }
                }

                return _instance;
            }
        }
    }

    public class SingletonException : Exception
    {
        public SingletonException(string exception)
        {
            throw new Exception(exception);
        }

        public SingletonException(Exception exception)
        {
            throw exception;
        }
    }
}
