using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Shared
{
    public static partial class CommonExtensions
    {
        private static JsonConverter _jsonConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
        private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { _jsonConverter },
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static void Map<TParent, TDerived>(ref TParent source, ref TDerived destination)
            where TParent : new()
            where TDerived : new()
        {
            if (source == null)
            {
                source = new TParent();
            }

            if (destination == null)
            {
                destination = new TDerived();
            }

            if (source == null || destination == null)
            {
                throw new Exception("Source or/and Destination Objects are null!");
            }

            var destinationType = destination.GetType();
            var sourceType = source.GetType();

            var properties_to_map = from sourceProperty in propertyCache[sourceType]
                                    let destinationProperty = destinationType.GetProperty(sourceProperty.Name)
                                    where sourceProperty.CanRead
                                    && destinationProperty != null
                                    && (destinationProperty.GetSetMethod(true) != null
                                    && !destinationProperty.GetSetMethod(true).IsPrivate)
                                    && (destinationProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                                    && destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)
                                    select new { mappedSource = sourceProperty, mappedDestination = destinationProperty };

            foreach (var properties in properties_to_map)
            {
                properties.mappedDestination.SetValue(destination, properties.mappedSource.GetValue(source, null), null);
            }

        }

        //Map properties from one instance of T to another by shape and types (not property names).
        public static T Map<T>(this T target, T source) => throw new NotImplementedException();

        //Map properties from one instance of T to another instance of U by shape and types(not property names).
        public static T Map<T, U>(this T target, U source)
            where T : class where U : class => throw new NotImplementedException();

        public static object Slurp(this object destination, object source)
        {
            var destinationType = destination.GetType();
            var sourceType = source.GetType();

            var mappableProperties = from sourceProperty in propertyCache[sourceType]
                                     let targetProperty = destinationType.GetProperty(sourceProperty.Name)
                                     where sourceProperty.CanRead
                                     && targetProperty != null
                                     && (targetProperty.GetSetMethod(true) != null
                                     && !targetProperty.GetSetMethod(true).IsPrivate)
                                     && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                                     && targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)
                                     select new { sourceProperty, targetProperty };

            foreach (var property in mappableProperties)
            {
                property.targetProperty.SetValue(destination, property.sourceProperty.GetValue(source, null), null);
            }

            return destination;
        }

        //Alias for Consume method because I like(d) the name        
        public static TParent Slurp<TParent, TDerived>(this TParent destination, TDerived source)
            where TParent : new()
            where TDerived : TParent, new()
        {
            if (source == null)
            {
                source = default(TDerived);
            }

            if (destination == null)
            {
                destination = default(TParent);
            }

            var destinationType = destination.GetType();
            var sourceType = source.GetType();

            var mappableProperties = from sourceProperty in propertyCache[sourceType]
                                     let destinationProperty = destinationType.GetProperty(sourceProperty.Name)
                                     where sourceProperty.CanRead
                                     && destinationProperty != null
                                     && (destinationProperty.GetSetMethod(true) != null
                                     && !destinationProperty.GetSetMethod(true).IsPrivate)
                                     && (destinationProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                                     && destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)
                                     select new { mappedSource = sourceProperty, mappedDestination = destinationProperty };

            foreach (var properties in mappableProperties)
            {
                properties.mappedDestination.SetValue(destination, properties.mappedSource.GetValue(source, null), null);
            }

            return destination;
        }

        public static TDefined Slurp<TDefined>(this TDefined destination, object source)
            where TDefined : class
        {
            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            var sourceProperties = propertyCache[sourceType];

            var mappableProperties = from sourceProperty in sourceProperties
                                     let destinationProperty = destinationType.GetProperty(sourceProperty.Name)
                                     where sourceProperty.CanRead
                                     && destinationProperty != null
                                     && (destinationProperty.GetSetMethod(true) != null
                                     && !destinationProperty.GetSetMethod(true).IsPrivate)
                                     && (destinationProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                                     && destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)
                                     select new { mappedSource = sourceProperty, mappedDestination = destinationProperty };

            foreach (var properties in mappableProperties)
            {
                properties.mappedDestination.SetValue(destination, properties.mappedSource.GetValue(source, null), null);
            }

            return destination;
        }

        //Combine 2 different classes into a current intance of R.
        public static R Merge<T, U, R>(this R result, T first, U second)
            where T : class where U : class where R : class => throw new NotImplementedException();

        //Combine 2 different class instances into a new instance, R.
        public static R Merge<T, U, R>(T first, U second)
            where T : class where U : class where R : new() => throw new NotImplementedException();

        //Combine 2 different class instances into an out instance, R.
        public static R Merge<T, U, R>(T first, U second, out R result) => throw new NotImplementedException();

        public static void ToPropertyDictionary(object @object) => propertyCache[@object?.GetType()]
            ?.ToDictionary(property => property.Name, property => property.GetValue(@object));

        public static void ToPropertyLookup(object @object) => propertyCache[@object?.GetType()]
            ?.ToLookup(property => property.Name, property => property.GetValue(@object));

        public static object GetPropertyValue<T>(this T @object, string propertyName) => propertyCache[typeof(T)]
            ?.Single(pi => pi.Name == propertyName)?.GetValue(@object, null);

        public static bool Compare(this object @object, object another)
        {
            if (ReferenceEquals(@object, another))
            {
                return true;
            }

            if (@object == null || another == null)
            {
                return false;
            }

            if (@object.GetType() != another.GetType())
            {
                return false;
            }

            bool result = true;

            if (!@object.GetType().IsClass)
            {
                return @object.Equals(another);
            }

            var properties = propertyCache[@object?.GetType()];

            foreach (var property in properties ?? Enumerable.Empty<PropertyInfo>())
            {
                object objValue = property.GetValue(@object);
                object anotherValue = property.GetValue(another);

                //Recursion
                if (!objValue.DeepCompare(anotherValue))
                {
                    result = false;
                }
            }

            return result;

        }

        public static bool JsonCompare(this object obj, object another)
        {
            if (ReferenceEquals(obj, another))
            {
                return true;
            }

            if ((obj == null) || (another == null))
            {
                return false;
            }

            if (obj.GetType() != another.GetType())
            {
                return false;
            }

            string objJson = JsonConvert.SerializeObject(obj);
            string anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }

        public static bool DeepCompare(this object obj, object another)
        {
            if (ReferenceEquals(obj, another))
            {
                return true;
            }

            if (obj == null || another == null)
            {
                return false;
            }

            //Compare two object's class, return false if they are difference
            if (obj.GetType() != another.GetType())
            {
                return false;
            }

            bool result = true;

            //Get all properties of obj
            //And compare each other
            var properties = propertyCache[obj?.GetType()];

            foreach (var property in properties ?? Enumerable.Empty<PropertyInfo>())
            {
                object objValue = property.GetValue(obj);
                object anotherValue = property.GetValue(another);

                if (!objValue.Equals(anotherValue))
                {
                    result = false;
                }
            }

            return result;
        }

        public static T Dump<T>(this T obj, string displayName = null, bool showNulls = true)
        {
            if (obj != null)
            {
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = obj.GetType().Name;
                }

                if (!showNulls)
                {
                    _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }

                string prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented, _jsonSerializerSettings);
                Debug.WriteLine(string.Format("{0}:\n{1}", displayName, prettyJson));
            }
            else if (obj == null)
            {
                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    Debug.WriteLine(string.Format("Object '{0}'{1}", displayName, " is null.")); //Optional
                }
            }

            return obj;
        }
    }
}
