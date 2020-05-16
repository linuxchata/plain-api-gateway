using System;

using NUnit.Framework;

using PlainApiGateway.Domain.Http;
using PlainApiGateway.Helper;

namespace PlainApiGateway.UnitTests.Helper
{
    [TestFixture]
    public class PlainHttpRequestHelperTests
    {
        [Test]
        public void When_get_url_And_plain_http_request_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => PlainHttpRequestHelper.GetUrl(null));
        }

        [Test]
        public void When_get_url_Then_returns_url()
        {
            //Arrange
            var plainHttpRequest = new PlainHttpRequest
            {
                Method = "GET",
                Scheme = "http",
                Host = "host",
                Port = 443,
                Path = "path",
                QueryString = "?userid=42"
            };

            //Act
            string result = PlainHttpRequestHelper.GetUrl(plainHttpRequest);

            //Assert
            Assert.That(result, Is.EqualTo("http://host:443/path?userid=42"));
        }
    }
}