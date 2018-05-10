using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common
{
    public static partial class CommonExtensions
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
