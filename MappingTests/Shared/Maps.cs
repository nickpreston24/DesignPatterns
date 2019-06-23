using System;
using System.Collections.Generic;

namespace Contract.Shared
{
    public class Maps<T>// : Dictionary<Type, Func<From, To>>
    {
        ////private Dictionary<Type, Func<From, To>> mappingStrategies = new Dictionary<Type, Func<From, To>>(0);
        //private Dictionary<T, Func<Type, Type>> mappingStrategies = new Dictionary<T, Func<Type, Type>>(0);

        //public Maps()
        //{
        //}

        //public Maps(Dictionary<Type, Func<Type, Type>> strategies) => mappingStrategies = strategies;
        ////public Maps(Dictionary<Type, Func<From, To>> strategies) => mappingStrategies = strategies;

        //public Func<Type, T> this[T type]
        //{
        //    get => mappingStrategies[type];
        //    set => mappingStrategies[type] = value;
        //}

        //public Func<From, T> Register(Type type, Func<From, T> func)
        //{
        //    mappingStrategies.Add(type, func);
        //    return func;
        //}

        //TODO: support Expressions

        //public Maps<From, To> Create() => this ?? new Maps<From, To>();

        //public Maps<From, To> Register<From, To>(Func<From, To> func)
        //{
        //    var success = mappingStrategies.TryAdd(typeof(To), func);
        //    return this;
        //}

        //private Func<From, To> GetMappingStrategy<From>()
        //{
        //    string strategyName = typeof(From)?.Name;
        //    return mappingStrategies[""];
        //}
    }
}
