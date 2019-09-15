using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Extensions.Tests
{
    [TestFixture()]
    public class RangeTests
    {
        [Test()]
        public void RangesAreUnequal()
        {
            Range a = new Range(1, 10);
            Range b = new Range(3, 8);

            var newRange = a.With(end: 15);
            Print(newRange, a, b);

            Console.WriteLine(a.ToString(false));
            Console.WriteLine(b.ToString(false));
            Console.WriteLine(newRange.ToString(false));

            Assert.AreEqual(newRange, a);
            Assert.AreNotEqual(a, b);
        }

        [Test()]
        public void Inclusiveness()
        {
            Range inclusiveEnded = new Range(1, 5);
            Range exclusiveStart = new Range(1, 5, false, true);
            Range bothInclusive = new Range(1, 5, true, true);
            Range bothExclusive = new Range(1, 5, false, false);

            Print(inclusiveEnded);
            Print(exclusiveStart);
            Print(bothInclusive);
            Print(bothExclusive);

            //Assert.AreEqual(inclusiveEnded[0], 1);
            //Assert.AreNotEqual(inclusiveEnded[4], 1);
            //Assert.AreEqual(inclusiveEnded[4], 5);

            //Assert.AreEqual(exclusiveStart[0], 1);
            //Assert.AreNotEqual(exclusiveStart[4], 1);
            //Assert.AreEqual(exclusiveStart[4], 5);
        }

        [Test()]
        public void DeconstructTest()
        {
            var range = new Range(1, 10);
            //var (Start, End) = range.Deconstruct(out Start, out End);

            throw new NotImplementedException();
        }

        [Test()]
        public void WithTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void DeconstructTest1()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void AsEnumerableTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void EqualsTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void EqualsTest1()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void GetHashCodeTest()
        {
            throw new NotImplementedException();
        }

        private void Print<T>(IEnumerable<T> collection) => Print(collection.ToList());

        private void Print<T>(List<T> list)
        {
            foreach (var obj in list)
                Print(obj);
        }

        private void Print<T>(params T[] collection)
        {
            foreach (var item in collection)
            {
                if (item is List<T> list)
                    Print(list);
                if (item is T[] array)
                    Print(array);

                Print(item);
            }
        }

        private void Print<T>(T item, bool debug = false)
        {
            Console.WriteLine(item?.ToString());
            Debug.WriteLineIf(debug, item?.ToString());
        }
    }
}