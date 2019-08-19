using NUnit.Framework;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Extensions.Tests
{
    [TestFixture()]
    public partial class ExtensionsTests
    {
        private static readonly int[] integers = new Range(1, 10);
        private int n = integers.Length;

        [Test()]
        public void CanSlice()
        {
            int start = integers.Take(n / 2).FirstRandom(), next = n - (n - start);
            Print(start, next);
            var result = integers.Slice(start, next);
            //Print(next, start);
            Print(result);
            Assert.True(result.Count(num => integers.Contains(num)) == next);
        }

        [Test()]
        public void CanTakeLast()
        {
            int last = new Range(1, n)
                .AsEnumerable()
                .FirstRandom();
            var result = integers.Slice(n - last, n);
            Print(result);
            var intersection = result.Intersect(integers);

            Assert.True(intersection.Count() == last);
        }

        [Test()]
        public void CanMaxBy()
        {
            bool evenGreater(int x) => x % 2 == 0 && x > 6;
            Expression<Func<int, int, int>> add = (x, y) => x + y;
            var adder = add.Compile();

            Print(adder(5, 6));

            int result = integers.MaxBy(evenGreater);
            //Print(integers);
            //Print(result);

            Assert.True(result > 0 && result <= integers.Max());
        }

        [Test()]
        public void CanMaxByTestWithComparer()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanMinBy()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanMinByTest1()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanToDatatable()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanAsEmpty()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanInterweave()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanInterweaveTest1()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanBatch()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanIsNullOrEmpty()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanToCsv()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanRunActionOn()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanTakeRandom()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanFirstRandom()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanMoveUp()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanGetItemsWhere()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanGetItemsWhereTest1()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanGetItemsWhereTest2()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanGetRandomItemsWhere()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanGetRandomItems()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanLeftJoin()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanRightJoin()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanFullOuterJoin()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanLeftExcludingJoin()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanRightExcludingJoin()
        {
            Assert.Inconclusive();
        }

        [Test()]
        public void CanFullOuterExcludingJoin()
        {
            Assert.Inconclusive();
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

        private void Print<T>(T item)
        {
            Console.WriteLine(item?.ToString());
            //Debug.WriteLine(item?.ToString());
        }
    }
}