using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Shared
{
    public static partial class CommonExtensions
    {
        public static T PopAt<T>(this IList<T> list, int index)
        {
            var result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static T Pop<T>(this IList<T> list, T item)
        {
            return list.Remove(item) ? item : throw new Exception($"{MethodBase.GetCurrentMethod().Name}: could not remove item");
        }

        public static IList<T> Pop<T>(this IList<T> list, int count)
        {
            var last = list.TakeLast(count).ToList();
            for (int i = 0; i < count; i++)
            {
                list.RemoveAt(list.Count - 1);
            }
            return last;
        }

        public static void AddMany<T>(this List<T> list, IEnumerable<T> elements)
        {
            list?.AddRange(elements ?? Enumerable.Empty<T>());
        }

        public static void BubbleSort<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    var object1 = list[j - 1];
                    var object2 = list[j];

                    if (((IComparable)object1).CompareTo(object2) > 0)
                    {
                        list.Remove(object1);
                        list.Insert(j, object1);
                    }
                }
            }
        }

        // Shuffles an IList in place.        
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            return list == null || list.Count == 0 ?
                list.Shuffle(new Random(Guid.NewGuid().GetHashCode())) : new List<T>();
        }

        // Shuffles an IList in place.
        public static IList<T> Shuffle<T>(this IList<T> list, Random random)
        {
            int count = list.Count;

            while (count > 1)
            {
                int i = random.Next(count--);
                var temp = list[count];
                list[count] = list[i];
                list[i] = temp;
            }

            return list;
        }

        // Append a single item of type T to a given list
        public static List<T> Append<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static DataTable ToDatatable<T>(this List<T> list, string name = "")
        {
            var table = string.IsNullOrWhiteSpace(name) ? new DataTable(typeof(T).Name) : new DataTable(name);

            if (list.Count == 0)
            {
                return table;
            }

            //var properties = typeof(T).GetProperties();
            var properties = propertyCache[typeof(T)];

            foreach (var property in properties)
            {
                table.Columns.Add(property.Name.SplitCamelCase(), property.PropertyType);
            }

            foreach (var item in list ?? Enumerable.Empty<T>())
            {
                object[] rowValues = new object[properties.Length];

                for (int i = 0; i < rowValues.Length; i++)
                {
                    rowValues[i] = properties[i].GetValue(item);
                }

                table.Rows.Add(rowValues);
            }

            return table;
        }

        public static IList<T> RemoveDuplicates<T>(this IList<T> items)
        {
            return items.Distinct().ToList();
        }
    }
}
