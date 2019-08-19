using Moq;
using Movies.Api.Graphs;
using NUnit.Framework;
using System;
using System.Linq;

namespace MoviesApi.Tests
{
    [TestFixture]
    public class ActorTests
    {
        private MockRepository mockRepository;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            mockRepository.VerifyAll();
        }

        private Actor CreateActor()
        {
            return new Actor
            {
                Id = Enumerable.Range(10, 99).FirstRandom()
            };
        }

        [Test]
        public void ToString_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var actor = CreateActor();

            // Act
            var result = actor.ToString();

            // Assert
            Assert.Fail();
        }

        [Test]
        public void WorksWith_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var actor = CreateActor();
            Actor otherActor = CreateActor();

            // Act
            //var result = actor.WorksWith(otherActor);

            // Assert
            actor.Films.AddRange(new[]
                {
                    new Movies.Api.Movie
                    {
                        MpaaRating = Movies.Shared.MPAARating.G,
                        Title = "Never Ending Story",
                        Rating = 3.5
                    },
                    new Movies.Api.Movie
                    {
                        MpaaRating = Movies.Shared.MPAARating.R,
                        Title = "The Matrix",
                        Rating = 4.3
                    }
                });

            Console.WriteLine(actor.ToString());
            Assert.IsNotNull(actor);
        }
    }
}