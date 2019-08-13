using System.Collections.Generic;
using System.Linq;

namespace System.Collections.ObjectModel
{
    public static partial class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> collection)
        {
            foreach (var item in collection ?? Enumerable.Empty<T>())
            {
                observableCollection.Add(item);
            }
        }
    }
}
