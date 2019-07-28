using Neo4j.Driver.V1;
using System;
using System.Linq;

namespace Movies.Client
{
    class HelloWorldExample : IDisposable
    {
        private readonly IDriver driver;

        public HelloWorldExample(string uri, string user, string password)
            => driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));

        public void PrintGreeting(string message)
        {
            using (var session = driver.Session())
            {
                var greeting = session.WriteTransaction(transaction =>
                {
                    var result = transaction.Run("CREATE (a:Greeting) " +
                                        "SET a.message = $message " +
                                        "RETURN a.message + ', from node ' + id(a)",
                        new { message });
                    return result.Single()[0].As<IRecord>();
                });
                Console.WriteLine(greeting);
            }
        }

        public void Dispose() => driver?.Dispose();

        public static void Main()
        {
            int port = 7687;
            using (var greeter = new HelloWorldExample($"bolt://localhost:{port}", "neo4j", "password"))
            {
                greeter.PrintGreeting("hello, world");
            }
        }
    }
}
