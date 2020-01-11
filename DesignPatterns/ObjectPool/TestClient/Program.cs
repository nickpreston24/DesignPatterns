using DesignPatterns;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pools.TestClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var token = new CancellationTokenSource();

            // Create an opportunity for the user to cancel.
            Task.Run(() =>
            {
                if (Console.ReadKey().KeyChar == 'c' || Console.ReadKey().KeyChar == 'C')
                    token.Cancel();
            });

            var pool = new ObjectPool<Person>(() => new Person());

            // Create a high demand for Person objects.
            int limit = 1000000;
            Parallel.For(0, limit, (i, loopState) =>
            {
                Person person = pool.Get();
                Console.CursorLeft = 0;
                // This is the bottleneck in our application. All threads in this loop
                // must serialize their access to the static Console class.
                //Console.WriteLine("Name: " + person.Name);
                Console.WriteLine("Pool count: " + pool.Count + " Thread Id: " + i);

                pool.Add(person);

                //Thread.Sleep(1050);
                if (token.Token.IsCancellationRequested)
                    loopState.Stop();
            });
            pool.Dispose();
            Console.WriteLine("Press the Enter key to exit.");
            Console.ReadLine();
            token.Dispose();
        }
    }

    internal class Person
    {
    }
}