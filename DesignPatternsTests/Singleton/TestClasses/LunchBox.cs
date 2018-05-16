using System.Diagnostics;

namespace DesignPatterns.Tests
{
    public class LunchBox : ISingleton<Apple>, ISingleton
    {
        private Apple apple;
        public string OwnerName { get; set; } = "Bob";
        public Selector<Apple> Selector { get; set; } = new Selector<Apple>();

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
