using System;

namespace DesignPatterns
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();//todo: Completes Updates/Saves of a Repo's private collection<T>.
    }
}