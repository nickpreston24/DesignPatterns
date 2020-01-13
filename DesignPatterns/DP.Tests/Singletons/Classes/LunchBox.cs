using System.Diagnostics;
using DesignPatterns.Singletons;

namespace DP.Tests.Singletons.Classes
{
    public class LunchBox : ISingleton
    {
        Apple apple;
        public string OwnerName { get; set; } = "Bob";

        public static LunchBox Instance => Singleton<LunchBox>.Instance;

        LunchBox() => apple = new Apple();

        internal void Open()
        {
            Debug.WriteLine(apple.ToString());
        }
    }

    public class Apple
    {
        public override string ToString() => "I am an apple! :D";
    }
}