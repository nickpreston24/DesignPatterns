using System;

namespace DesignPatterns
{
    public class SingletonException : Exception
    {
        public SingletonException(string exception) => throw new Exception(exception);

        public SingletonException(Exception exception) => throw exception;
    }
}
