using Moq;
using Movies.Api.Graphs;
using NUnit.Framework;

namespace MoviesApi.Tests
{
    [TestFixture]
    public class PlayedInTests
    {
        private MockRepository mockRepository;

        private Mock<Actor> mockActor;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockActor = mockRepository.Create<Actor>();
        }

        [TearDown]
        public void TearDown()
        {
            mockRepository.VerifyAll();
        }

        private PlayedIn CreatePlayedIn()
        {
            return new PlayedIn(mockActor.Object);
        }

        [Test]
        public void Condition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playedIn = CreatePlayedIn();

            // Act
            var result = playedIn.Condition();

            // Assert
            Assert.Fail();
        }
    }
}