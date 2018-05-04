using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common.Extensions
{
    public static partial class Extensions
    {
        public static void WaitAll(this IEnumerable<Thread> threads)
        {
            foreach (var thread in threads ?? Enumerable.Empty<Thread>())
            {
                thread.Join();
            }
        }
    }
}
