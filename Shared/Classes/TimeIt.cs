﻿using System.Collections.Generic;
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

            int units = spans[timeSpanUnit](elapsed);
            string unitName = timeSpanUnit.GetDescription();
            string message = $"{name} took {units} {unitName}";

            Console.WriteLine(message);
            Debug.WriteLine(message);
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