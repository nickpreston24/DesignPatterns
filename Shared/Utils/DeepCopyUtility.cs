using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CottonwoodFinancial.Framework.Data
{
    public static class DeepCopyUtility
    {
        public static object CopyObjectData(object objSource)
        {
            Type typeSource = objSource.GetType();
            object objTarget = objTarget = Activator.CreateInstance(typeSource);
            CopyObjectData(objSource, objTarget);
            return objTarget;
        }
        public static void CopyObjectData(object source, object target)
        {
            CopyObjectData(source, target, String.Empty, BindingFlags.Public | BindingFlags.Instance);
        }
        public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
            {
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                if (string.IsNullOrEmpty(excludedProperties) == false
                    && excluded.Contains(name))
                {
                    continue;
                }

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo sourcefield = source.GetType().GetField(name);
                    if (sourcefield == null) { continue; }

                    object SourceValue = sourcefield.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    try
                    {
                        PropertyInfo piTarget = Field as PropertyInfo;
                        PropertyInfo sourceField = source.GetType().GetProperty(name, memberAccess);
                        if (sourceField == null) { continue; }

                        if (piTarget.CanWrite && sourceField.CanRead)
                        {
                            object targetValue = piTarget.GetValue(target, null);
                            object sourceValue = sourceField.GetValue(source, null);

                            if (sourceValue == null) { continue; }

                            if (sourceField.PropertyType.IsArray
                                && piTarget.PropertyType.IsArray
                                && sourceValue != null)
                            {
                                CopyArray(source, target, memberAccess, piTarget, sourceField, sourceValue);
                            }
                            else if (typeof(System.Collections.ICollection).IsAssignableFrom(sourceField.PropertyType)
                                && typeof(System.Collections.ICollection).IsAssignableFrom(piTarget.PropertyType)
                                && sourceValue != null
                                && sourceField.PropertyType != typeof(string))
                            {
                                CopyList(source, target, memberAccess, piTarget, sourceField, sourceValue);
                            }
                            else
                            {
                                CopySingleData(source, target, memberAccess, piTarget, sourceField, targetValue, sourceValue);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        private static void CopySingleData(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object targetValue, object sourceValue)
        {
            if (targetValue == null
                && piTarget.PropertyType.IsValueType == false
                && piTarget.PropertyType != typeof(string))
            {
                if (piTarget.PropertyType.IsArray)
                {
                    targetValue = Activator.CreateInstance(piTarget.PropertyType.GetElementType());
                }
                else
                {
                    targetValue = Activator.CreateInstance(piTarget.PropertyType);
                }
            }

            if (piTarget.PropertyType.IsValueType == false
                && piTarget.PropertyType != typeof(string))
            {
                CopyObjectData(sourceValue, targetValue, "", memberAccess);
                piTarget.SetValue(target, targetValue, null);
            }
            else
            {
                if (piTarget.PropertyType.FullName == sourceField.PropertyType.FullName)
                {
                    object tempSourceValue = sourceField.GetValue(source, null);
                    piTarget.SetValue(target, tempSourceValue, null);
                }
                else
                {
                    CopyObjectData(piTarget, target, "", memberAccess);
                }
            }
        }
        private static void CopyArray(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object sourceValue)
        {
            int sourceLength = (int)sourceValue.GetType().InvokeMember("Length", BindingFlags.GetProperty, null, sourceValue, null);
            Array targetArray = Array.CreateInstance(piTarget.PropertyType.GetElementType(), sourceLength);
            Array array = (Array)sourceField.GetValue(source, null);

            for (int i = 0; i < array.Length; i++)
            {
                object o = array.GetValue(i);
                object tempTarget = Activator.CreateInstance(piTarget.PropertyType.GetElementType());
                CopyObjectData(o, tempTarget, "", memberAccess);
                targetArray.SetValue(tempTarget, i);
            }
            piTarget.SetValue(target, targetArray, null);
        }
        private static void CopyList(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object sourceValue)
        {
            try
            {
                if (sourceField.PropertyType.IsGenericType)
                {
                    Type genericType = sourceField.PropertyType.GetGenericArguments().First();
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(genericType);
                    IList newList = (IList)Activator.CreateInstance(constructedListType);

                    ICollection sourceList = sourceValue as ICollection;
                    foreach (var obj1 in sourceList)
                    {
                        object obj2 = Activator.CreateInstance(obj1.GetType());
                        CopyObjectData(obj1, obj2, "", memberAccess);
                        newList.Add(obj2);
                    }

                    piTarget.SetValue(target, newList, null);
                }
                else
                {
                    
                }
            }
            catch
            {

            }
        }
    }
}