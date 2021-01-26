using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

using NUnit.Framework;

using PlainApiGateway.Configuration;
using PlainApiGateway.Provider.Configuration;

namespace PlainApiGateway.UnitTests.Provider.Configuration
{
    [TestFixture]
    public class PlainRouteConfigurationProviderTests
    {
        private PlainRouteConfigurationProvider sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var loggerFactory = new NullLoggerFactory();

            this.sut = new PlainRouteConfigurationProvider(loggerFactory);
        }

        [Test]
        public void When_get_matching_And_routes_null_Then_throws_argument_null_exception()
        {
            //Arrange
            var defaultHttpContext = new DefaultHttpContext();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.sut.GetMatching(null, defaultHttpContext.Request));
        }

        [Test]
        public void When_get_matching_And_http_request_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.sut.GetMatching(new List<PlainRouteConfiguration>(), null));
        }

        [Test]
        public void When_get_matching_And_matching_configuration_not_found_Then_returns_null()
        {
            //Arrange
            var configurations = new List<PlainRouteConfiguration>
            {
                new PlainRouteConfiguration
                {
                    Source = new PlainRouteSourceConfiguration
                    {
                        HttpMethods = new []{ "GET" },
                        PathTemplate = "/comment"
                    }
                }
            };

            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = "GET";
            defaultHttpContext.Request.Path = "/post/1";

            //Act
            var result = this.sut.GetMatching(configurations, defaultHttpContext.Request);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void When_get_matching_And_http_methods_mismatch_Then_returns_null()
        {
            //Arrange
            var configurations = new List<PlainRouteConfiguration>
            {
                new PlainRouteConfiguration
                {
                    Source = new PlainRouteSourceConfiguration
                    {
                        HttpMethods = new []{ "POST" },
                        PathTemplate = "/post"
                    }
                }
            };

            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = "GET";
            defaultHttpContext.Request.Path = "/post/1";

            //Act
            var result = this.sut.GetMatching(configurations, defaultHttpContext.Request);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void When_get_matching_And_target_addresses_not_found_Then_returns_null()
        {
            //Arrange
            var configurations = new List<PlainRouteConfiguration>
            {
                new PlainRouteConfiguration
                {
                    Source = new PlainRouteSourceConfiguration
                    {
                        HttpMethods = new []{ "POST" },
                        PathTemplate = "/post"
                    },
                    Target = new PlainRouteTargetConfiguration()
                }
            };

            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = "POST";
            defaultHttpContext.Request.Path = "/post/1";

            //Act
            var result = this.sut.GetMatching(configurations, defaultHttpContext.Request);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void When_get_matching_And_matching_route_found_Then_returns_plain_route_configuration()
        {
            //Arrange
            var configuration = new PlainRouteConfiguration
            {
                Source = new PlainRouteSourceConfiguration
                {
                    HttpMethods = new[] { "POST" },
                    PathTemplate = "/post"
                },
                Target = new PlainRouteTargetConfiguration
                {
                    Addresses = new[]
                    {
                        new PlainRouteTargetAddressConfiguration
                        {
                            Host = "localhost",
                            Port = 36
                        }
                    }
                }
            };

            var configurations = new List<PlainRouteConfiguration> { configuration };

            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = "POST";
            defaultHttpContext.Request.Path = "/post/1";

            //Act
            var result = this.sut.GetMatching(configurations, defaultHttpContext.Request);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(configuration));
        }
    }
}