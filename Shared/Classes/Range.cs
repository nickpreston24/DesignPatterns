using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class Range : IEquatable<Range>
    {
        private int[] array;
        private bool endInclusive = false;
        private bool startInclusive = true;

        public Range()
            : this(1, 1)
        {
        }

        public Range(int start, int end, bool startInclusive = true, bool endInclusive = false)
        {
            this.startInclusive = startInclusive;
            this.endInclusive = endInclusive;

            //Update(start, end, startInclusive, endInclusive);
            Update(start, end, (s) => Start = startInclusive ? start : start + 1, (e) => End = endInclusive ? end : end - 1);
            array = Enumerable.Range(Start, End).ToArray();
        }

        private void Update(int start, int end, Action<int> updateStart, Action<int> updateEnd)
        {
            updateStart = updateStart ?? ((_) => Start = start);
            updateEnd = updateEnd ?? ((_) => End = end);
            if (start > 0) updateStart(start);
            if (end > 0) updateEnd(end);
        }

        private void Update(int start, int end, bool startInclusive, bool endInclusive)
        {
            if (start > 0)
                Start = startInclusive ? start : start + 1;
            if (end > 0)
                End = endInclusive ? end : end - 1;
        }

        public int End { get; internal set; }
        public int Start { get; internal set; }
        public int this[int index] => array[index];

        public static implicit operator int[](Range range) =>
            Enumerable
            .Range(range.Start, range.End - range.Start + 1)
            .ToArray();

        public static implicit operator List<int>(Range range) =>
            Enumerable
            .Range(range.Start, range.End - range.Start + 1)
            .ToList();

        public static bool operator !=(Range left, Range right) => !(left == right);

        public static bool operator ==(Range left, Range right) => left.Equals(right);

        public IEnumerable<int> AsEnumerable()
        {
            int[] result = new Range(Start, End, startInclusive, endInclusive);
            return result;
        }

        public void Deconstruct(ref int Start, ref int End, ref bool startInclusive, ref bool endInclusive)
        {
            this.Start = Start;
            this.End = End;
            this.startInclusive = startInclusive;
            this.endInclusive = endInclusive;
        }

        public (int, int) Deconstruct() => (Start, End);

        public override bool Equals(object obj)
            => obj is Range range && Equals(range);

        public bool Equals(Range other)
            => Start == other.Start
            && End == other.End
            && startInclusive == other.startInclusive
            && endInclusive == other.endInclusive;

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => (startInclusive ? $"[{Start}" : $"({Start}")
                .Append(endInclusive ? $"..{End})" : $"..{End}]");

        public string ToString(bool rangeFormat = true)
            => rangeFormat
                ? ToString()
                : string.Join(",", array.Select(n => n.ToString()));

        public Range With(int start = -1, int end = -1) => WithNewOrDefaults(start, end);

        private Range WithNewOrDefaults(int nextStart, int nextEnd)
        {
            Console.WriteLine($"old start {Start}, old end {End}");
            if (nextStart > 0)
                Start = nextStart;
            if (nextEnd > 0)
                End = nextEnd;
            Console.WriteLine($"next start {nextStart}, next end {nextEnd}");
            return new Range(Start, End, startInclusive, endInclusive);
        }
    }
}