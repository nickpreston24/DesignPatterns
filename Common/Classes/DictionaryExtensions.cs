
namespace Shared
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV defaultValue = default(TV))
        {
            return dictionary.TryGetValue(key, out TV value) ? value : defaultValue;
        }
    }
}
