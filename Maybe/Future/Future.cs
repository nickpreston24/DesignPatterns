using System;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Source: https://mikhail.io/2018/07/monads-explained-in-csharp-again/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Future<T>
    {
        private readonly Task<T> currentTask;

        public Future(T instance) => currentTask = Task.FromResult(instance);

        private Future(Task<T> nextTask) => currentTask = nextTask;

        public Future<U> Bind<U>(Func<T, Future<U>> func)
        {
            var nextTask = currentTask
                .ContinueWith(task => func(task.Result).currentTask)
                .Unwrap();
            return new Future<U>(nextTask);
        }

        public void OnComplete(Action<T> action) => currentTask.ContinueWith(task => action(task.Result));
    }
}