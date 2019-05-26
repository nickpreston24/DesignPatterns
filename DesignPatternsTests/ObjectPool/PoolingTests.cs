using DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsTests
{
    [TestClass]
    public class PoolingTests
    {
        private int[] counts = new int[] { 5, 10, 100, 0, 1/*, -1 */};

        [TestMethod]
        public void DSLTest()
        {
            var pool = new ObjectPool<Person>()
                .SetGenerator(() => default)
                .Add(new Person { Age = 29, Name = "Michael Preston" })
                .Add(Enumerable.Range(0, 999)
                    .Aggregate(new List<Person>(), func: (result, age) =>
                    {
                        result.Add(new Person { Age = age });
                        return result;
                    }));

            pool.Dump("DSL Generated Pool");
        }

        [TestMethod]
        public void CanCreateObjectPool()
        {
            foreach (var expected in counts)
            {
                var pool = CreatePeople(expected);

                pool.Get().Dump(nameof(Person));

                int remaining = pool.Count;
                Debug.WriteLine("Remaining: " + remaining);

                pool.Dispose();
                Assert.AreEqual(0, pool.Count);
            }
        }

        private static ObjectPool<Person> CreatePeople(int expected)
        {
            var pool = new ObjectPool<Person>(expected);
            for (int i = 0; i < expected; i++)
                pool.Add(new Person() { Name = "#" + i });
            return pool;
        }

        [TestMethod]
        public void CanPreloadPool()
        {
            int actual;
            foreach (int expected in counts)
            {
                var pool = new ObjectPool<Person>(expected);
                actual = pool.Count;

                //Assert
                Debug.WriteLine("Expected: " + expected, "Actual: " + actual);
                Assert.AreEqual(actual, expected);

                pool.Add(new Person());
                Debug.WriteLine("new count: " + pool.Count);

                pool.Dispose();
                //pool.Dump("disposed pool");
                Assert.AreEqual(0, pool.Count);
            }
        }

        [TestMethod]
        public void CanGetMany()
        {
            foreach (var expected in counts)
            {
                var pool = new ObjectPool<Person>();
                pool.Get().Dump("The one");
                pool.Get(5).Dump("Up to five");
            }
        }

        //TODO: move to a Debug program for best visualization!
        [TestMethod]
        public void CanConcurrentlyPool()
        {
            foreach (int expected in counts)
            {
                var pool = new ObjectPool<Person>(expected);

                //Assert
                int actual = pool.Count;
                Debug.WriteLine("Expected: " + expected, "Actual: " + actual);
                Assert.AreEqual(actual, expected);

                pool.Dispose();
                Assert.AreEqual(0, pool.Count);
            }
        }

        //[TestMethod]
        //public void CanFindAndRemoveItem()
        //{
        //    //
        //    /// Remove specific items
        //    ////

        //    //var removed = pool.GetRandom(expected / 2);
        //    //Debug.WriteLine($"Removed {removed.Count()} items");

        //    //foreach (var item in removed)
        //    //    pool.Release(item);
        //    //pool.Dump("pre-fail");

        //    Assert.Inconclusive();
        //}
    }
}