using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Movies.Client
{
    public class HelloWorldExample : IDisposable
    {
        private readonly IDriver driver;

        public HelloWorldExample(string uri, string user, string password)
            => driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));

        public void KingMe()
        {
            using (var session = driver.Session())
            {
                //Make a king
                session.Run("CREATE (a:Person {name:'Arthur', title:'King'})");

                //Search for a king:
                var result = session.Run("MATCH (a:Person) WHERE a.name = 'Arthur' RETURN a.name AS name, a.title AS title");

                Print(result);
            }
        }

        public void Print(params IRecord[] results)
        {
            foreach (IRecord record in results)
            {
                Console.WriteLine($"{record["title"].As<string>()} {record["name"].As<string>()}");
            }
        }

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

        public IEnumerable<IRecord> RunQuery(string query)
        {
            using (var session = driver.Session())
            {
                return session.Run(query);
            }
        }

        public void Dispose() => driver?.Dispose();

        public static void Main()
        {
            int port = 7687;
            using (var greeter = new HelloWorldExample($"bolt://localhost:{port}", "neo4j", "root"))
            {
                greeter.PrintGreeting("hello, world");
                greeter.KingMe();
            }
        }

        private void Print(params object[] array)
        {
            foreach (var item in array)
            {
                if (item is IEnumerable<object> list)
                    Print(list);

                Print(item);
            }
        }

        private void Print(object item)
        {
            if (item is IEnumerable<object> list)
                Print(list.ToArray());

            Console.WriteLine(item?.ToString());
            Debug.WriteLine(item?.ToString());
        }
    }
}