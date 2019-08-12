﻿using Delegates;
using System;
using System.Timers;
using static System.Timers.TimeIt;

namespace FasterThanReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] properties = new string[] { "Number", "Text", "Money" };
            var instance = new MyClass { Number = 25, Text = "lol", Money = 12d };
            var type = typeof(MyClass);

            using (var timer = new TimeIt(TimeSpanUnit.Milliseconds))
            {
                foreach (var property in properties)
                {
                    Console.WriteLine(DelegateFactory.PropertyGet(type, property)(instance));
                }
            }
        }
    }

    public class MyClass
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public double Money { get; set; }
    }
}