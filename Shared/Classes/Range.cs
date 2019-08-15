using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public struct Range
    {
        public int Start { get; }
        public int End { get; }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public static implicit operator int[](Range range) =>
            Enumerable
            .Range(range.Start, range.End - range.Start + 1)
            .ToArray();

        public static implicit operator List<int>(Range range) =>
            Enumerable
            .Range(range.Start, range.End - range.Start + 1)
            .ToList();

        public IEnumerable<int> AsEnumerable()
        {
            int[] result = new Range(Start, End);
            return result;
        }
    }
}