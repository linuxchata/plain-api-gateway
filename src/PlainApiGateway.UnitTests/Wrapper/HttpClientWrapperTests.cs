using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Net.Http.Headers;

using Moq;
using Moq.Protected;

using NUnit.Framework;

using PlainApiGateway.Wrapper;

namespace PlainApiGateway.UnitTests.Wrapper
{
    [TestFixture]
    public class HttpClientWrapperTests
    {
        private const string RequestUrl = "https://example.com";

        private const string HttpGetMethod = "GET";

        private const string HttpPostMethod = "POST";

        private const int TimeoutInSeconds = 5;

        private Mock<IHttpClientFactory> httpClientFactoryMock;

        private HttpClientWrapper sut;

        [SetUp]
        public void SetUp()
        {
            this.httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var loggerFactory = new NullLoggerFactory();

            this.sut = new HttpClientWrapper(this.httpClientFactoryMock.Object, loggerFactory);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void When_send_request_And_request_url_invalid_Then_throws_argument_null_exception(string requestUrl)
        {
            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.sut.SendRequest(requestUrl, HttpGetMethod, null, new HeaderDictionary(), TimeoutInSeconds));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void When_send_request_And_http_method_invalid_Then_throws_argument_null_exception(string httpMethod)
        {
            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.sut.SendRequest(RequestUrl, httpMethod, null, new HeaderDictionary(), TimeoutInSeconds));
        }

        [Test]
        public void When_send_request_And_headers_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => this.sut.SendRequest(RequestUrl, HttpGetMethod, null, null, TimeoutInSeconds));
        }

        [Test]
        public async Task When_send_request_And_request_body_empty_Then_returns_response()
        {
            //Arrange
            var clientHandlerMock = new Mock<DelegatingHandler>();
            clientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            clientHandlerMock.As<IDisposable>().Setup(a => a.Dispose());

            var client = new HttpClient(clientHandlerMock.Object);

            this.httpClientFactoryMock.Setup(a => a.CreateClient(It.IsAny<string>())).Returns(client);

            var headers = new HeaderDictionary
            {
                { HeaderNames.Accept, MediaTypeNames.Application.Json }
            };

            //Act
            var response = await this.sut.SendRequest(RequestUrl, HttpGetMethod, null, headers, TimeoutInSeconds);

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task When_send_request_And_request_body_not_empty_Then_returns_response()
        {
            //Arrange
            var clientHandlerMock = new Mock<DelegatingHandler>();
            clientHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            clientHandlerMock.As<IDisposable>().Setup(a => a.Dispose());

            var client = new HttpClient(clientHandlerMock.Object);

            this.httpClientFactoryMock.Setup(a => a.CreateClient(It.IsAny<string>())).Returns(client);

            var requestBody = new StringContent("{ \"parameter\": \"value\" }");
            var requestBodyStream = await requestBody.ReadAsStreamAsync();

            var headers = new HeaderDictionary { ContentLength = requestBodyStream.Length };
            headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);

            //Act
            var response = await this.sut.SendRequest(RequestUrl, HttpPostMethod, requestBodyStream, headers, TimeoutInSeconds);

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}