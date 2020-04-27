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
        [TestCase("/post/1/2/3/4/5")]
        public void When_is_match_And_source_path_template_with_one_variable_And_path_matches_Then_returns_true(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase("/v/all")]
        [TestCase("/v/all/")]
        [TestCase("/v/1")]
        [TestCase("/v/1/")]
        [TestCase("/v/?userid=1")]
        [TestCase("/v/")]
        [TestCase("/v//")]
        [TestCase("/v/post/all")]
        [TestCase("/v/post/all/")]
        [TestCase("/v/post/1")]
        [TestCase("/v/post/1/")]
        [TestCase("/v/post?userid=1")]
        [TestCase("/v/post")]
        [TestCase("/v/post/")]
        [TestCase("/v/post/1/2/3/4/5")]
        [TestCase("/v1/all")]
        [TestCase("/v1/all/")]
        [TestCase("/v1/1")]
        [TestCase("/v1/1/")]
        [TestCase("/v1/?userid=1")]
        [TestCase("/v1/")]
        [TestCase("/v1//")]
        [TestCase("/v1/post/all")]
        [TestCase("/v1/post/all/")]
        [TestCase("/v1/post/1")]
        [TestCase("/v1/post/1/")]
        [TestCase("/v1/post?userid=1")]
        [TestCase("/v1/post")]
        [TestCase("/v1/post/")]
        [TestCase("/v1/post/1/2/3/4/5")]
        public void When_is_match_And_source_path_template_with_two_variables_And_path_matches_Then_returns_true(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/v{version}/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase("/post/all")]
        [TestCase("/post/all/")]
        [TestCase("/post/1")]
        [TestCase("/post/1/")]
        [TestCase("/post/")]
        [TestCase("/post/1/2/3/4/5")]
        public void When_is_match_And_source_path_template_with_one_variable_and_prefix_And_path_matches_Then_returns_true(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/post/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase("/v/post/all")]
        [TestCase("/v/post/all/")]
        [TestCase("/v/post/1")]
        [TestCase("/v/post/1/")]
        [TestCase("/v/post/")]
        [TestCase("/v2/post/all")]
        [TestCase("/v2/post/all/")]
        [TestCase("/v2/post/1")]
        [TestCase("/v2/post/1/")]
        [TestCase("/v2/post/")]
        [TestCase("/v2/post/1/2/3/4/5")]
        public void When_is_match_And_source_path_template_with_two_variables_and_prefix_And_path_matches_Then_returns_true(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/v{version}/post/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.True);
        }

        [TestCase("")]
        [TestCase("/all")]
        [TestCase("/all/")]
        [TestCase("/1")]
        [TestCase("/1/")]
        [TestCase("/?userid=1")]
        [TestCase("/")]
        [TestCase("//")]
        [TestCase("/post")]
        [TestCase("/post?userid=1")]
        public void When_is_match_And_source_path_template_with_one_variable_and_prefix_And_path_does_not_match_Then_returns_false(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/post/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.False);
        }

        [TestCase("/v/all")]
        [TestCase("/v/all/")]
        [TestCase("/v/1")]
        [TestCase("/v/1/")]
        [TestCase("/v/?userid=1")]
        [TestCase("/v/")]
        [TestCase("/v//")]
        [TestCase("/v/post?userid=1")]
        [TestCase("/v/post")]
        [TestCase("/v3/all")]
        [TestCase("/v3/all/")]
        [TestCase("/v3/1")]
        [TestCase("/v3/1/")]
        [TestCase("/v3/?userid=1")]
        [TestCase("/v3/")]
        [TestCase("/v3//")]
        [TestCase("/v3/post?userid=1")]
        [TestCase("/v3/post")]
        public void When_is_match_And_source_path_template_with_two_variables_and_prefix_And_path_does_not_match_Then_returns_false(string requestPath)
        {
            //Arrange
            var sourcePathTemplate = "/v{version}/post/{any}";

            //Act
            var result = PathMatchHelper.IsMatch(sourcePathTemplate, requestPath);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}