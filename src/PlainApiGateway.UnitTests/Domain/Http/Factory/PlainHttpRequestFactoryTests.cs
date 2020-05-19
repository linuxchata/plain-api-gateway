using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

using PlainApiGateway.Configuration;
using PlainApiGateway.Domain.Http.Factory;
using PlainApiGateway.Provider.Http;

namespace PlainApiGateway.UnitTests.Domain.Http.Factory
{
    [TestFixture]
    public class PlainHttpRequestFactoryTests
    {
        private const string Scheme = "http";

        private const string Host = "localhost";

        private const int Port = 1433;

        private const string Path = "/path";

        private const string QueryString = "?userid=1";

        private const ushort TimeoutInSeconds = 5;

        private PlainHttpRequestFactory sut;

        [SetUp]
        public void SetUp()
        {
            var httpRequestPathProviderMock = new Mock<IHttpRequestPathProvider>();

            var nullLoggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();

            this.sut = new PlainHttpRequestFactory(httpRequestPathProviderMock.Object, nullLoggerFactory);
        }

        [Test]
        public void When_create_Then_returns_plain_http_request()
        {
            //Arrange
            var plainRouteConfiguration = new PlainRouteConfiguration
            {
                Target = new PlainRouteTargetConfiguration
                {
                    Addresses = new[]
                    {
                        new PlainRouteTargetAddressConfiguration
                        {
                            Host = Host,
                            Port = Port
                        }
                    },
                    Scheme = Scheme
                },
                Source = new PlainRouteSourceConfiguration()
            };

            //Act
            var result = this.sut.Create(HttpMethods.Get, Path, QueryString, new HeaderDictionary(0), TimeoutInSeconds, plainRouteConfiguration);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Headers, Is.Not.Null);
            Assert.That(result.Method, Is.EqualTo(HttpMethods.Get));
            Assert.That(result.Scheme, Is.EqualTo(Scheme));
            Assert.That(result.Host, Is.EqualTo(Host));
            Assert.That(result.Port, Is.EqualTo(Port));
            Assert.That(result.Path, Is.EqualTo(null));
            Assert.That(result.QueryString, Is.EqualTo(QueryString));
            Assert.That(result.TimeoutInSeconds, Is.EqualTo(TimeoutInSeconds));
        }
    }
}