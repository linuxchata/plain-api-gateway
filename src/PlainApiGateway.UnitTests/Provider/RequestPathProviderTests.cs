using System;

using NUnit.Framework;

using PlainApiGateway.Provider;

namespace PlainApiGateway.UnitTests.Provider
{
    [TestFixture]
    public class RequestPathProviderTests
    {
        private RequestPathProvider sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequestPathProvider();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void When_get_And_http_request_path_invalid_Then_throws_argument_null_exception(string httpRequestPath)
        {
            //Arrange
            var sourcePathTemplate = "/values/{any}";
            var targetPathTemplate = "/values/{any}";

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => this.sut.Get(httpRequestPath, sourcePathTemplate, targetPathTemplate));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void When_get_And_source_path_template_invalid_Then_throws_argument_null_exception(string sourcePathTemplate)
        {
            //Arrange
            var httpRequestPath = "/values/all";
            var targetPathTemplate = "/values/{any}";

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => this.sut.Get(httpRequestPath, sourcePathTemplate, targetPathTemplate));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void When_get_And_target_path_template_invalid_Then_throws_argument_null_exception(string targetPathTemplate)
        {
            //Arrange
            var httpRequestPath = "/values/all";
            var sourcePathTemplate = "/values/{any}";

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => this.sut.Get(httpRequestPath, sourcePathTemplate, targetPathTemplate));
        }

        [TestCase("/post/all")]
        [TestCase("/post/1")]
        [TestCase("/post?userid=1")]
        [TestCase("/post")]
        [TestCase("/post/3")]
        public void When_get_And_source_and_target_path_template_are_same_Then_returns_request_path(string httpRequestPath)
        {
            //Arrange
            var sourcePathTemplate = "/post{any}";
            var targetPathTemplate = "/post{any}";

            //Act
            var result = this.sut.Get(httpRequestPath, sourcePathTemplate, targetPathTemplate);

            //Assert
            Assert.That(result, Is.EqualTo(httpRequestPath));
        }

        [TestCase("/post/all", "/post/api/all")]
        [TestCase("/post/1", "/post/api/1")]
        [TestCase("/post?userid=1", "/post/api?userid=1")]
        [TestCase("/post", "/post/api")]
        [TestCase("/post/3", "/post/api/3")]
        public void When_get_And_source_and_target_path_template_are_different_Then_returns_request_path(string httpRequestPath, string expectedRequestPath)
        {
            //Arrange
            var sourcePathTemplate = "/post{any}";
            var targetPathTemplate = "/post/api{any}";

            //Act
            var result = this.sut.Get(httpRequestPath, sourcePathTemplate, targetPathTemplate);

            //Assert
            Assert.That(result, Is.EqualTo(expectedRequestPath));
        }
    }
}