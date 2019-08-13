using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Shared
{
    public static partial class DynamicExtensions
    {
        private static StringComparison _comparison = StringComparison.OrdinalIgnoreCase;

        //todo: put in your XmlMapper class
        public static T Extract<T>(string xml) where T : class
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return default(T);
            }

            string className = typeof(T).Name;
            var xmlDocument = XDocument.Parse(xml);
            string node = xmlDocument.GetFirstNode(className);
            xmlDocument = null;
            xmlDocument = XDocument.Parse(node);
            var dictionary = xmlDocument.Root.ToDynamic() as ExpandoObject;
            var result = dictionary.ToInstance<T>();
            return result as T;
        }

        public static T ToInstance<T>(this IDictionary<string, object> dictionary) where T : class
        {
            var type = typeof(T);
            var instance = (T)ToInstance(Activator.CreateInstance(type, true), dictionary, type);
            return instance;
        }

        public static object ToInstance(this IDictionary<string, object> dictionary, Type type)
        {
            object instance = ToInstance(Activator.CreateInstance(type, true), dictionary, type);
            return instance;
        }

        private static object ToInstance(object parent, IDictionary<string, object> dictionary, Type childType)
        {
            var parentType = parent.GetType();
            string parentTypeName = parentType.Name;

            var parentProperties = parentType.GetProperties();
            var childProperties = childType.GetProperties();

            //Debug.WriteLine($"Parent Type: {parentTypeName}");

            foreach (var pair in dictionary ?? new Dictionary<string, object>(0))
            {
                //Debug.WriteLine($"On key {pair.Key}");
                if (pair.Value == null)
                {
                    Debug.WriteLine($"Info: Skipped key '{pair.Key}'");
                    continue;
                }

                object value = pair.Value;
                var valType = pair.Value.GetType();

                try
                {
                    //Debug.WriteLine($"Class: {parentTypeName}\tElement: {pair.Key.ToString()} raw Value: {pair.Value.ToString()}\ttype: {valType.ToString()}");

                    //todo: if list of objects that are not defined and NOT expandos, must be a list<object>, just iterate blindly in another method that handles list of object and calls toIntance when encountering expandos.                    

                    if (!valType.Name.Equals(nameof(ExpandoObject)))
                    {
                        var nextProperty = childProperties
                            .SingleOrDefault(childProperty => childProperty.Name.Equals(pair.Key, _comparison));

                        //var nextProperty = childProperties
                        //   .SingleOrDefault(childProperty => childProperty.Name.Equals(pair.Key, _comparison)
                        //   || childProperty.PropertyType.Name.Equals(pair.Key, _comparison));

                        nextProperty?.SetValue(parent, TypeDescriptor.GetConverter(nextProperty.PropertyType)
                                .ConvertFrom(pair.Value), null);

                        continue;
                    }

                    IDictionary<string, object> nextChildDictionary = pair.Value as ExpandoObject;
                    string propertyName = pair.Key.ToString();
                    //error?: pair.Key is not the name of the Property, but the name of the property type. :(

                    if (propertyName.Equals(parentTypeName, _comparison))
                    {
                        object childTemplate = Activator.CreateInstance(childType, true);
                        object child = ToInstance(parent: childTemplate, dictionary: nextChildDictionary, childType: childType);

                        parent = child;
                    }
                    else
                    {
                        var nextProperty = childProperties
                            .SingleOrDefault(pi => pi.Name.Equals(propertyName, _comparison)
                                || pi.PropertyType.Name.Equals(propertyName, _comparison));

                        if (nextProperty == null)
                        {
                            Debug.WriteLine($"Info: Could not find property '{propertyName}'");
                            continue;
                        }

                        var nextChildType = nextProperty?.PropertyType;
                        //Debug.WriteLine($"next child type: {nextChildType?.Name}");

                        object nextChildInstance = CreateChild(nextChildDictionary, nextChildType);
                        var childBaseType = nextChildInstance.GetType();

                        //Debug.WriteLine($"Child base type: {childBaseType.Name}");
                        //#if !(NET20 || NET35 || NET40 || NET45 || NET451 || NET452)
                        //if (nextChildInstance is IEnumerable list)
                        //{
                        //    nextProperty.SetValue(parent, list);
                        //}
                        //#else
                        if (nextChildInstance is IEnumerable)
                        {
                            var list = nextChildInstance as IEnumerable;
                            nextProperty.SetValue(parent, list);
                        }
                        //#endif
                        else
                        {
                            nextProperty.SetValue(parent, nextChildInstance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = $"Encountered an error when converting to instance of '{parentTypeName}' from element '{pair.Key.ToString()}':\n {ex.Message}\nEnsure XML elements match the instance schema!";

                    //string message = string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.ToString());
                    //string message = string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message);

                    Debug.WriteLine(message);
                    continue;
                }
            }

            return parent;
        }

        /// <summary>
        /// From: https://stackoverflow.com/questions/13171525/converting-xml-to-a-dynamic-c-sharp-object
        /// Or
        /// https://www.codeproject.com/Articles/461677/Creating-a-dynamic-object-from-XML-using-ExpandoOb
        /// </summary>
        /// <param name="xDocument"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this XDocument xDocument) => xDocument.Root.ToDynamic() as IDictionary<string, object>;

        public static dynamic ToDynamic(this XElement node)
        {
            var parent = new ExpandoObject();
            return node.ToDynamic(parent);
        }

        private static dynamic ToDynamic(this XElement node, dynamic parent)
        {
            if (node.HasElements)
            {
                if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                {
                    var item = new ExpandoObject();
                    var list = new List<dynamic>();
                    foreach (var element in node.Elements())
                    {
                        ToDynamic(element, list);
                    }

                    AddProperty(item, node.Elements().First().Name.LocalName, list);
                    AddProperty(parent, node.Name.ToString(), item);
                }
                else
                {
                    var item = new ExpandoObject();

                    foreach (var attribute in node.Attributes())
                    {
                        AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                    }

                    foreach (var element in node.Elements())
                    {
                        ToDynamic(element, item);
                    }

                    AddProperty(parent, node.Name.ToString(), item);
                }
            }
            else
            {
                AddProperty(parent, node.Name.ToString(), node.Value.Trim());
            }

            return parent;
        }
        private static object CreateChild(IDictionary<string, object> childDictionary, Type childType)
        {
            object child = null;

            if (childType.BaseType.Equals(typeof(Array)))
            {
                var elementType = childType.GetElementType();
                var list = new List<object>(childDictionary.Values.Count);

                foreach (IEnumerable expandos in childDictionary.Values)
                {
                    foreach (ExpandoObject expando in expandos)
                    {
                        object next = ToInstance(expando, elementType);
                        list.Add(next);
                    }
                }

                //child = list as IEnumerable;
                var childArray = Array.CreateInstance(elementType, list.Count);
                object[] source = list.Cast<object>().ToArray();
                Array.Copy(source, childArray, list.Count);

                child = childArray;

            }
            else
            {
                object parent = Activator.CreateInstance(childType, true);
                child = ToInstance(parent, childDictionary, childType) ?? Activator.CreateInstance(childType);
            }

            return child;
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                (parent as List<dynamic>).Add(value);
            }
            else
            {
                (parent as IDictionary<string, object>)[name] = value;
            }
        }

        //#region Need Testing
        //public static dynamic ToDynamic(this IDictionary<string, object> dictionary)
        //{
        //    dynamic expando = new ExpandoObject();
        //    var expandoDictionary = (IDictionary<string, object>)expando;

        //    //todo: make recursive for objects in the dictionary with more levels.
        //    dictionary.ToList()
        //              .ForEach(keyValue => expandoDictionary.Add(keyValue.Key, keyValue.Value));

        //    return expando;
        //}

        //public static dynamic ToDynamic<T>(this T obj)
        //{
        //    IDictionary<string, object> expando = new ExpandoObject();
        //    var properties = typeof(T).GetProperties();

        //    foreach (var propertyInfo in properties ?? Enumerable.Empty<PropertyInfo>())
        //    {
        //        var propertyExpression = Expression.Property(Expression.Constant(obj), propertyInfo);
        //        string currentValue = Expression.Lambda<Func<string>>(propertyExpression).Compile().Invoke();
        //        expando.Add(propertyInfo.Name.ToLower(), currentValue);
        //    }
        //    return expando as ExpandoObject;
        //}

        //public static DataTable ToDataTable(this List<dynamic> list)
        //{
        //    DataTable table = new DataTable();
        //    var properties = list.GetType().GetProperties();
        //    properties = properties.ToList().GetRange(0, properties.Count() - 1).ToArray();
        //    properties.ToList().ForEach(p => table.Columns.Add(p.Name, typeof(string)));
        //    list.ForEach(x => table.Rows.Add(x.Name, x.Phone));
        //    return table;
        //}
        //#endregion Need Testing
    }

    public static partial class DynamicExtensions
    {
        public static T DeserializeFromXML<T>(this string xml)
          where T : class
        {
            using (TextReader reader = new StringReader(xml))
            {
                return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
            }
        }

        public static string SerializeToXml<T>(this T @object)
            where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, @object);
                    return writer.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public static class XDocumentExtensions
    {
        public static string GetFirstNode(this XDocument xmlDocument, string nodeTag)
        {
            var node = (from xml in xmlDocument.Descendants(nodeTag)
                        select xml).FirstOrDefault();

            return node?.ToString();
        }

        public static XElement GetFirstElement(this XElement xElement, string tag) => xElement.Elements(tag).FirstOrDefault() ?? xElement;

        public static XElement GetFirstDescendant(this XElement xElement, string tag) => xElement.Descendants(tag).FirstOrDefault() ?? xElement;

        public static XElement GetFirstDescendant(this XDocument xmlDocument, string tag) => xmlDocument.Descendants(tag).FirstOrDefault() ?? xmlDocument.Root;

        public static XElement GetFirstElement(this XDocument xmlDocument, string tag) => xmlDocument.Descendants(tag).FirstOrDefault() ?? xmlDocument.Root;

        public static object CreateClass(this Assembly assembly, XDocument xmlDocument, string className)
        {
            var classType = assembly.FindType(className);
            string xml = xmlDocument.GetFirstNode(classType.Name);

            var dict = XDocument.Parse(xml).ToDynamic() as IDictionary<string, object>;
            object result = dict.ToInstance(classType);
            return result;
        }

        public static Type FindType(this Assembly assembly, string typeName) => assembly.GetTypes().Where(type => type.Name.Equals(typeName)).SingleOrDefault();
    }

    public class DynamicAliasAttribute : Attribute
    {
        public string Alias { get; set; }
        public DynamicAliasAttribute(string alias) => Alias = alias;
    }
}
