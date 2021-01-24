using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

using NUnit.Framework;

using PlainApiGateway.Cache;
using PlainApiGateway.Configuration;
using PlainApiGateway.Domain.Http;
using PlainApiGateway.Domain.Http.Factory;
using PlainApiGateway.Handler;
using PlainApiGateway.Middleware;
using PlainApiGateway.Provider.Configuration;
using PlainApiGateway.Wrapper;

namespace PlainApiGateway.UnitTests.Middleware
{
    [TestFixture]
    public class RequestRedirectMiddlewareTests
    {
        private const string HttpMethod = "GET";

        private const string HttpScheme = "https";

        private const string Host = "example.com";

        private const ushort Port = 443;

        private const string Path = "/path";

        private const string QueryString = "?clientId=42";

        private DefaultHttpContext context;

        private Mock<IErrorHandler> errorHandlerMock;

        private Mock<IPlainConfigurationCache> plainConfigurationCacheMock;

        private Mock<IPlainRouteConfigurationProvider> plainRouteConfigurationProviderMock;

        private Mock<IPlainHttpRequestFactory> plainHttpRequestFactoryMock;

        private Mock<IHttpClientWrapper> httpClientWrapperMock;

        private RequestRedirectMiddleware sut;

        [SetUp]
        public void SetUp()
        {
            this.context = new DefaultHttpContext();
            this.context.Request.Method = HttpMethod;
            this.context.Request.Path = Path;
            this.context.Request.QueryString = new QueryString(QueryString);

            this.errorHandlerMock = new Mock<IErrorHandler>();

            this.plainConfigurationCacheMock = new Mock<IPlainConfigurationCache>();
            this.plainRouteConfigurationProviderMock = new Mock<IPlainRouteConfigurationProvider>();

            this.plainHttpRequestFactoryMock = new Mock<IPlainHttpRequestFactory>();

            this.httpClientWrapperMock = new Mock<IHttpClientWrapper>();

            var loggerFactory = new NullLoggerFactory();

            this.sut = new RequestRedirectMiddleware(
                RequestDelegate,
                plainConfigurationCacheMock.Object,
                plainRouteConfigurationProviderMock.Object,
                plainHttpRequestFactoryMock.Object,
                httpClientWrapperMock.Object,
                loggerFactory);
        }

        [Test]
        public void When_invoke_async_And_matching_route_not_found_Then_does_not_throw_exception()
        {
            //Arrange
            var plainConfiguration = new PlainConfiguration
            {
                TimeoutInSeconds = 10,
                Routes = new List<PlainRouteConfiguration> { new() }
            };

            this.plainConfigurationCacheMock.Setup(a => a.Get()).Returns(plainConfiguration);

            this.plainRouteConfigurationProviderMock
                .Setup(a => a.GetMatching(It.IsAny<List<PlainRouteConfiguration>>(), It.IsAny<HttpRequest>()))
                .Returns((PlainRouteConfiguration)null);

            //Assert
            Assert.DoesNotThrowAsync(() => this.sut.InvokeAsync(this.context, errorHandlerMock.Object));

            this.errorHandlerMock.Verify(a => a.SetRouteNotFoundErrorResponse(It.IsAny<HttpContext>()), Times.Once);
        }

        [Test]
        public void When_invoke_async_And_request_returns_ok_status_code_Then_returns_ok_status_code()
        {
            //Arrange
            var plainConfiguration = new PlainConfiguration
            {
                TimeoutInSeconds = 10,
                Routes = new List<PlainRouteConfiguration> { new() }
            };

            this.plainConfigurationCacheMock.Setup(a => a.Get()).Returns(plainConfiguration);

            this.plainRouteConfigurationProviderMock
                .Setup(a => a.GetMatching(It.IsAny<List<PlainRouteConfiguration>>(), It.IsAny<HttpRequest>()))
                .Returns(plainConfiguration.Routes[0]);

            this.plainHttpRequestFactoryMock
                .Setup(a => a.Create(
                    It.IsNotNull<string>(),
                    It.IsNotNull<string>(),
                    It.IsNotNull<string>(),
                    It.IsNotNull<IHeaderDictionary>(),
                    It.IsNotNull<ushort>(),
                    It.IsNotNull<PlainRouteConfiguration>()))
                .Returns(new PlainHttpRequest
                {
                    Method = HttpMethod,
                    Scheme = HttpScheme,
                    Host = Host,
                    Port = Port,
                    Path = Path,
                    QueryString = QueryString,
                    TimeoutInSeconds = plainConfiguration.TimeoutInSeconds.Value,
                });

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            this.httpClientWrapperMock
                .Setup(a => a.SendRequest(
                    It.IsNotNull<string>(),
                    It.IsNotNull<string>(),
                    It.IsNotNull<Stream>(),
                    It.IsAny<IHeaderDictionary>(),
                    It.IsNotNull<int>()))
                .ReturnsAsync(httpResponseMessage);

            //Assert
            Assert.DoesNotThrowAsync(() => this.sut.InvokeAsync(this.context, errorHandlerMock.Object));
            Assert.That(this.context.Response.StatusCode, Is.EqualTo(200));

            this.errorHandlerMock.Verify(a => a.SetRouteNotFoundErrorResponse(It.IsAny<HttpContext>()), Times.Never);
        }

        public async Task RequestDelegate(HttpContext httpContext)
        {
            await Task.CompletedTask;
        }
    }
}