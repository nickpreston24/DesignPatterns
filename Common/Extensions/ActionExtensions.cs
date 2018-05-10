using System;
using System.Threading.Tasks;

namespace Common
{
    public static partial class CommonExtensions
    {
        public static Task AsTask(this Action action) => new Task(action);

        public static Task AsTask<T>(this Func<T> action) => new Task<T>(action);

        public static void With<T>(this T item, Action<T> work) => work(item);
    }
}
