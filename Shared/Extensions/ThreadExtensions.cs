using System.Collections.Generic;
using System.Linq;

namespace System.Threading
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
