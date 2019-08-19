using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public struct Range : IEquatable<Range>
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

        public static bool operator ==(Range left, Range right) => left.Equals(right);

        public static bool operator !=(Range left, Range right) => !(left == right);

        public IEnumerable<int> AsEnumerable()
        {
            int[] result = new Range(Start, End);
            return result;
        }

        public override bool Equals(object obj) => obj is Range range && Equals(range);

        public bool Equals(Range other) => Start == other.Start && End == other.End;

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            return hashCode;
        }
    }
}