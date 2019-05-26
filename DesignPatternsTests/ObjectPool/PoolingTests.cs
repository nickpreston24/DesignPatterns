using DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System;
using System.Diagnostics;
using System.Linq;

namespace DesignPatternsTests
{
    [TestClass]
    public class PoolingTests
    {
        [TestMethod]
        public void CanCreateObjectPool()
        {
            var pool = new ObjectPool<Person>();
            var numPeople = 100;
            for (int i = 0; i < numPeople; i++)
            {
                pool.Add(new Person() { Name = "#" + i });
            }

            var removed = pool.Instances.TakeRandom(10);
            Debug.WriteLine($"Removed {removed.Count()} items");

            foreach (var item in removed)
            {
                pool.Release(item);
            }

            int remaining = pool.Count;
            Debug.WriteLine("Remaining: " + remaining);
            //pool.Instances.Skip(10).Take(5).Dump();            //pool.Instances.Skip(10).Take(5).Dump();
        }

        [TestMethod]
        public void CanPreloadPool()
        {
            int expected = 5;
            var pool = new ObjectPool<Person>(expected);
            int actual = pool.Instances.Count;

            Debug.WriteLine("Expected: " + expected, "Actual: " + actual);
            Assert.AreEqual(actual, expected);

            pool.Add(new Person());
            Debug.WriteLine("new count: " + pool.Instances.Count);
        }
    }
}
