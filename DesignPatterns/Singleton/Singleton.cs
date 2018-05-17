using System;
using System.Reflection;

namespace DesignPatterns
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
                        instance.Selector = instance.Selector ?? new Selector();
                    }
                }

                return instance;
            }
        }
    }


}
