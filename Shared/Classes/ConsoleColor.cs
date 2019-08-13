using System;

namespace Shared.Classes
{
    public class ConsoleColor : IDisposable
    {
        private readonly System.ConsoleColor previousColor;
        public ConsoleColor(System.ConsoleColor color)
        {
            previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }
        public void Dispose() => Console.ForegroundColor = previousColor;
    }
}
