using System;

namespace DesignPatterns.Singletons
{
    public class SingletonException : Exception
    {
        public SingletonException(string exception) => throw new Exception(exception);

        public SingletonException(Exception exception) => throw exception;
    }
}
