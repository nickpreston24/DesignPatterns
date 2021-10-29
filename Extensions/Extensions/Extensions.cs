namespace Common.Extensions
{
    public static partial class Extensions
    {
        // A default property cache.  More may be created, ad-hoc
        public static PropertyCache PropertyCache = new PropertyCache();


        static string[] None = new string[] { };

        /// <summary>
        /// Runs the specified action upon specified value.
        /// Returns value as-is, even if change by the action.
        /// Good for side-effects not granted by your library or .NET.
        /// </summary>
        /// Usage:
        /// <usage> 
        /// var myBigMultiLevelObject.With(o=>{
        ///     o.State.City.Address1.Zip = 12345
        ///     
        ///     ...
        ///     DoSomething();
        ///     });
        /// DoSomethingElse();
        /// </usage>
        public static T With<T>(this T self, Action<T> action)
            where T : class
        {
            action(self);
            return self;
        }

        /// <summary>
        /// Alters the specified value using a function.
        /// Returns any mutation as-is.
        /// 
        /// Usage:
        /// <usage> 
        /// var myBigMultiLevelObjectList.Alter(list=>{
        ///     list.AddRange(...)
        ///     ...
        ///     DoSomethingElse();
        ///     
        ///     return list;
        ///     });
        /// ...
        /// </usage>
        /// </summary>
        public static T Alter<T>(this T self, Func<T, T> action)
            where T : class
                => self != null ? action(self) : default;

        public static string[] GetNullProperties<T>(this T instance, bool getNullsOnly = true)
            where T : class, new()
        {
            return instance == null
                ? None
                : propertyCache.Stash(typeof(T))
                   .Where(prop => getNullsOnly
                        ? prop.GetValue(instance, null) == null
                        : prop.GetValue(instance, null) != null)
                   .Select(p => p.Name)
                   .ToArray();
        }

        public static bool HasNullProperty<T>(this T instance, string propertyName = "")
            where T : class, new() => instance
                .GetNullProperties()
                    .Any(name => name == propertyName);

        public static bool HasNullProperties<T>(this T instance)
            where T : class, new() => instance != null
                && propertyCache.Stash(typeof(T))
                   .Any(prop => prop.GetValue(instance, null) == null);
    }
}
