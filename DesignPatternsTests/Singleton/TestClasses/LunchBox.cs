using System.Diagnostics;

namespace DesignPatterns.Tests
{
    public class LunchBox : ISingleton
    {
        private Apple apple;
        public string OwnerName { get; set; } = "Bob";
        public ISelector Selector { get; set; }

        public static LunchBox Instance
        {
            get { return Singleton<LunchBox>.Instance; }
        }

        private LunchBox()
        {
            apple = new Apple();
        }

        internal void Open()
        {
            Debug.WriteLine(apple.ToString());
        }
    }
    public class Apple
    {
        public override string ToString()
        {
            return "I am an apple! :D";
        }
    }
}
