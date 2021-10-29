using System;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static partial class Extensions
    {
        public static Task AsTask(this Action action) => new Task(action);

        public static Task AsTask<T>(this Func<T> action) => new Task<T>(action);

        public static void With<T>(this T item, Action<T> work) => work(item);
    }
}
