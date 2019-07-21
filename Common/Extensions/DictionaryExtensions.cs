using System.Linq;

namespace System.Collections.Generic
{
    public static partial class Extensions
    {
        //https://damieng.com/blog/2012/10/29/8-things-you-probably-didnt-know-about-csharp
        public static object GetValue<TKey>(this Dictionary<TKey, object> dictionary, TKey key) => dictionary[key];
        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dictionary, TK key) => dictionary.TryGetValue(key, out var value) ? value : default;

        //https://damieng.com/blog/2012/10/29/8-things-you-probably-didnt-know-about-csharp
        public static IEnumerable<object> GetValues<TKey>(this Dictionary<TKey, object> dictionary, params TKey[] keys) => keys.Select(key => dictionary[key]).AsEnumerable();
    }
}
