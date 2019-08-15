using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Timers
{
    public class TimeIt : IDisposable
    {
        private readonly string name;
        private readonly TimeSpanUnit timeSpanUnit;
        private readonly Stopwatch watch;
        private TimeSpan elapsed;
        private int units;

        public TimeSpan Elapsed => elapsed;

        public static TimeIt GetTimer(
            TimeSpanUnit timeSpanUnit = TimeSpanUnit.Milliseconds,
            [CallerMemberName] string name = ""
        )
        => new TimeIt(timeSpanUnit, name);

        private TimeIt()
        {
        }

        private TimeIt(TimeSpanUnit timeSpanUnit = TimeSpanUnit.Milliseconds, [CallerMemberName] string name = "")
        {
            this.name = name;
            this.timeSpanUnit = timeSpanUnit;
            watch = Stopwatch.StartNew();
        }

        private static Dictionary<TimeSpanUnit, Func<TimeSpan, int>> spans = new Dictionary<TimeSpanUnit, Func<TimeSpan, int>>()
        {
            [TimeSpanUnit.Default] = ts => ts.Milliseconds,
            [TimeSpanUnit.Minutes] = ts => ts.Days,
            [TimeSpanUnit.Hours] = ts => ts.Hours,
            [TimeSpanUnit.Minutes] = ts => ts.Minutes,
            [TimeSpanUnit.Seconds] = ts => ts.Seconds,
            [TimeSpanUnit.Milliseconds] = ts => ts.Milliseconds,
            //[TimeSpanUnit.Ticks] = ts => ts.Ticks, //TODO: add a cast to long in this dictionary somehow
        };

        public void Dispose()
        {
            watch.Stop();
            elapsed = watch.Elapsed;
            units = spans[timeSpanUnit](elapsed);
            Print();
        }

        private void Print()
        {
            //Don't print zeroes:
            if (units == 0)
                return;

            Console.WriteLine(ToString());
            Debug.WriteLine(ToString());
        }

        public override string ToString()
        {
            string unitName = timeSpanUnit.GetDescription();
            return $"{name} took {units} {unitName}";
        }

        public enum TimeSpanUnit
        {
            Default,
            Minutes,
            Hours,
            Seconds,
            Milliseconds,
            //Ticks,
        }
    }
}