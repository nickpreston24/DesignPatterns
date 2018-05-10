using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static partial class CommonExtensions
    {
        //https://damieng.com/blog/2012/10/29/8-things-you-probably-didnt-know-about-csharp
        public static object GetValue<TKey>(this Dictionary<TKey, object> dictionary, TKey key) => dictionary[key];

        //https://damieng.com/blog/2012/10/29/8-things-you-probably-didnt-know-about-csharp
        public static IEnumerable<object> GetValues<TKey>(this Dictionary<TKey, object> dictionary, params TKey[] keys) => keys.Select(key => dictionary[key]).AsEnumerable();
    }
}
