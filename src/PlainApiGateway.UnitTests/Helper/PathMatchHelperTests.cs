using NUnit.Framework;

using PlainApiGateway.Helper;

namespace PlainApiGateway.UnitTests.Helper
{
    [TestFixture]
    public class PathMatchHelperTests
    {
        [TestCase("/all")]
        [TestCase("/all/")]
        [TestCase("/1")]
        [TestCase("/1/")]
        [TestCase("/?userid=1")]
        [TestCase("/")]
        [TestCase("//")]
        [TestCase("/post/all")]
        [TestCase("/post/all/")]
        [TestCase("/post/1")]
        [TestCase("/post/1/")]
        [TestCase("/post?userid=1")]
        [TestCase("/post")]
        [TestCase("/post/")]
        public void When_is_match_And_any_path_And_path_matches_Then_returns_true(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase("/post/all")]
        [TestCase("/post/all/")]
        [TestCase("/post/1")]
        [TestCase("/post/1/")]
        [TestCase("/post?userid=1")]
        [TestCase("/post")]
        [TestCase("/post/")]
        public void When_is_match_And_any_path_with_prefix_And_path_matches_Then_returns_true(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/post/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase("/all")]
        [TestCase("/all/")]
        [TestCase("/1")]
        [TestCase("/1/")]
        [TestCase("/?userid=1")]
        [TestCase("/")]
        [TestCase("//")]
        public void When_is_match_And_any_path_with_prefix_And_path_does_not_match_Then_returns_false(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/post/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}