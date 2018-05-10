using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common
{
    public static partial class CommonExtensions
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
