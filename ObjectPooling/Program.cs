using DesignPatterns;
using Shared.Classes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectPooling
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            // Create an opportunity for the user to cancel.
            Task.Run(() =>
            {
                if (Console.ReadKey().KeyChar == 'c' || Console.ReadKey().KeyChar == 'C')
                    cts.Cancel();
            });

            var pool = new ObjectPool<Person>(() => new Person());

            // Create a high demand for Person objects.
            Parallel.For(0, 1000000, (i, loopState) =>
            {
                var person = pool.Get();
                Console.CursorLeft = 0;
                // This is the bottleneck in our application. All threads in this loop
                // must serialize their access to the static Console class.
                //Console.WriteLine("Name: " + person.Name);
                Console.WriteLine("Pool count: " + pool.Count + " Thread Id: " + i);

                pool.Add(person);

                //Thread.Sleep(1050);
                if (cts.Token.IsCancellationRequested)
                    loopState.Stop();
            });
            pool.Dispose();
            Console.WriteLine("Press the Enter key to exit.");
            Console.ReadLine();
            cts.Dispose();
        }
    }
}
