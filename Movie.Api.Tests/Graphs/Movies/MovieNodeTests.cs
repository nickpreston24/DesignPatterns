using Moq;
using Movies.Api;
using Movies.Api.Graphs;
using NUnit.Framework;

namespace MoviesApi.Tests
{
    [TestFixture]
    public partial class MovieNodeTests
    {
        private MockRepository mockRepository;

        private Mock<Movie> mockMovie;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockMovie = mockRepository.Create<Movie>();
        }

        [TearDown]
        public void TearDown()
        {
            mockRepository.VerifyAll();
        }

        private MovieNode CreateMovieNode()
        {
            return new MovieNode(mockMovie.Object);
        }

        [Test]
        public void ToString_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieNode = CreateMovieNode();

            // Act
            var result = movieNode.ToString();

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Dispose_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var movieNode = CreateMovieNode();
            bool disposing = false;

            // Act
            movieNode.Dispose(disposing);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Dispose_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var movieNode = CreateMovieNode();

            // Act
            movieNode.Dispose();

            // Assert
            Assert.Fail();
        }
    }
}