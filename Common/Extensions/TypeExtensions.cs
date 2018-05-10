using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Common
{
    public static partial class CommonExtensions
    {
        public static IEnumerable<Type> GetAssignableTypes<T>()
        {
            try
            {
                var assignableTypes = (from type in Assembly.Load(typeof(T).Namespace).GetExportedTypes()
                                       where !type.IsInterface && !type.IsAbstract
                                       where typeof(T).IsAssignableFrom(type)
                                       select type).ToArray();

                return assignableTypes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Dictionary<Type, UInterface> GetRepositories<TDerived, UInterface>()
            where UInterface : class, TDerived
        {
            return Assembly.GetAssembly(typeof(TDerived)).GetTypes()
               .Where(type => type.BaseType != null &&
                           type.BaseType.GetGenericArguments().FirstOrDefault() != null)
               .ToDictionary(type => type.BaseType.GetGenericArguments().FirstOrDefault(),
                            type => Activator.CreateInstance(type) as UInterface);

        }

        // Invoke Method from an Instance (non-null)
        public static void InvokeMethod<T>(this T instance, string instanceMethodName, object[] parameters)
        {
            try
            {
                var type = typeof(T);
                var toInvoke = type.GetMethod(instanceMethodName);
                Debug.WriteLine(toInvoke.Invoke(instance, parameters));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        // Invoke Static Method from a given Type
        public static void InvokeMethod(this Type classType, string staticMethodName, object[] parameters)
        {
            try
            {
                var toInvoke = classType.GetMethod(staticMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (toInvoke != null)
                {
                    toInvoke.Invoke(null, parameters);
                }
                else
                {
                    Debug.WriteLine($"Method '{staticMethodName}' not found in class '{classType.FullName}'!");//Make sure you are calling a static method!
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        // Invoke Generic Method from a given class Type on a new type 'genericMethodType'        
        public static void InvokeMethod(this Type classType, string genericMethodName, object[] parameters, Type genericMethodType)
        {
            try
            {
                object instance = Activator.CreateInstance(classType);
                var openMethod = classType.GetMethod(genericMethodName);
                var toInvoke = openMethod.MakeGenericMethod(genericMethodType);
                toInvoke.Invoke(instance, parameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
