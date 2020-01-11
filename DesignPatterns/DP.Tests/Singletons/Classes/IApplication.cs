﻿namespace DesignPatterns
{
    public interface IApplication : ISingleton
    {
        string Name { get; }
        string Version { get; }
    }
}