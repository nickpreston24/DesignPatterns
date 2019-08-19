using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CottonwoodFinancial.Framework.Data
{
    public class CompareObjects
    {
        public static List<string> CompareObjectData(object source, object target)
        {
            return CompareObjectData(source, target, BindingFlags.Public | BindingFlags.Instance);
        }

        public static List<string> CompareObjectData(object source, object target, BindingFlags memberAccess)
        {
            //if (!originalObjectProps[s].GetAccessors()[0].IsVirtual)
            var difference = new List<string>();
            if (source != null && target != null)
            {
                MemberInfo[] miT = target.GetType().GetMembers(memberAccess);

                foreach (MemberInfo Field in miT)
                {
                    string name = Field.Name;

                    if (Field.MemberType == MemberTypes.Field)
                    {
                        try
                        {
                            FieldInfo sourcefield = source.GetType().GetField(name);
                            FieldInfo targetfield = target.GetType().GetField(name);

                            if (sourcefield != null && targetfield != null)
                            {
                                object SourceValue = sourcefield.GetValue(source);
                                object tempTargetValue = targetfield.GetValue(target);

                                if (tempTargetValue != null || SourceValue != null)
                                {
                                    if (tempTargetValue == null || SourceValue == null)
                                    {
                                        difference.Add(sourcefield.FieldType.FullName);
                                    }
                                    else
                                    {
                                        if (!SourceValue.Equals(tempTargetValue))
                                        {
                                            difference.Add(string.Format("{0}.{1}", source.GetType().ToString(), sourcefield.Name));
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    else if (Field.MemberType == MemberTypes.Property)
                    {
                        try
                        {
                            var piTarget = Field as PropertyInfo;
                            PropertyInfo sourceField = source.GetType().GetProperty(name, memberAccess);

                            if (sourceField != null && piTarget != null)
                            {
                                if (sourceField.CanRead)
                                {
                                    object targetValue = piTarget.GetValue(target, null);
                                    object sourceValue = sourceField.GetValue(source, null);
                                    if (targetValue != null && sourceValue != null)
                                    {
                                        if (targetValue == null && sourceValue != null)
                                        {
                                            difference.Add(string.Format("{0}.{1}", source.GetType().ToString(), sourceField.Name));
                                        }
                                        else
                                        {
                                            if (sourceField.PropertyType.IsArray
                                                && piTarget.PropertyType.IsArray)
                                            {
                                                difference.AddRange(CompareArray(source, target, memberAccess, piTarget, sourceField, sourceValue));
                                            }
                                            else if (typeof(System.Collections.ICollection).IsAssignableFrom(sourceField.PropertyType)
                                                && typeof(System.Collections.ICollection).IsAssignableFrom(piTarget.PropertyType)
                                                && sourceField.PropertyType != typeof(string))
                                            {
                                                difference.AddRange(CompareList(source, target, memberAccess, piTarget, sourceField, sourceValue));
                                            }
                                            else
                                            {
                                                difference.AddRange(CompareSingleData(source, target, memberAccess, piTarget, sourceField, targetValue, sourceValue));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return difference;
        }

        private static List<string> CompareSingleData(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object targetValue, object sourceValue)
        {
            var difs = new List<string>();
            if (piTarget.PropertyType.IsValueType == false
                && piTarget.PropertyType != typeof(string))
            {
                difs.AddRange(CompareObjectData(sourceValue, targetValue, memberAccess));
            }
            else
            {
                if (piTarget.PropertyType.FullName == sourceField.PropertyType.FullName)
                {
                    try
                    {
                        object tempSourceValue = sourceField.GetValue(source, null);
                        object tempTargetValue = piTarget.GetValue(target, null);
                        if (tempTargetValue != null && tempSourceValue != null)
                        {
                            if (tempTargetValue == null && tempSourceValue != null)
                            {
                                difs.Add(string.Format("{0}.{1}", source.GetType().ToString(), sourceField.Name));
                            }
                            else
                            {
                                if (!tempSourceValue.Equals(tempTargetValue))
                                {
                                    difs.Add(string.Format("{0}.{1}", source.GetType().ToString(), sourceField.Name));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    difs.AddRange(CompareObjectData(piTarget, target, memberAccess));
                }
            }
            return difs;
        }

        private static List<string> CompareArray(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object sourceValue)
        {
            var difs = new List<string>();
            try
            {
                int sourceLength = (int)sourceValue.GetType().InvokeMember("Length", BindingFlags.GetProperty, null, sourceValue, null);
                var targetArray = (Array)piTarget.GetValue(target, null);
                var array = (Array)sourceField.GetValue(source, null);

                for (int i = 0; i < array.Length; i++)
                {
                    try
                    {
                        object o = array.GetValue(i);
                        object tempTarget = targetArray.GetValue(i);
                        difs.AddRange(CompareObjectData(o, tempTarget, memberAccess));
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return difs;
        }

        private static List<string> CompareList(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object sourceValue)
        {
            var difs = new List<string>();
            try
            {
                int sourceLength = (int)sourceValue.GetType().InvokeMember("Count", BindingFlags.GetProperty, null, sourceValue, null);
                var sourceList = sourceValue as ICollection;
                var targetList = piTarget.GetValue(target, null) as ICollection;

                int current1Index = 0;
                foreach (var obj1 in sourceList)
                {
                    int current2Index = 0;
                    foreach (var obj2 in targetList)
                    {
                        if (current1Index == current2Index)
                        {
                            difs.AddRange(CompareObjectData(obj1, obj2, memberAccess));
                            break;
                        }
                        current2Index++;
                    }
                    current1Index++;
                }
            }
            catch
            {
            }

            return difs;
        }
    }
}