using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

using NUnit.Framework;

using PlainApiGateway.Extension;
using PlainApiGateway.Middleware;

namespace PlainApiGateway.UnitTests.Middleware
{
    [TestFixture]
    public class ResponseMiddlewareTests
    {
        private const string HttpMethod = "GET";

        private const string Path = "/path";

        private const string QueryString = "?clientId=42";

        private DefaultHttpContext context;

        private ResponseMiddleware sut;

        [SetUp]
        public void SetUp()
        {
            this.context = new DefaultHttpContext();
            this.context.Request.Method = HttpMethod;
            this.context.Request.Path = Path;
            this.context.Request.QueryString = new QueryString(QueryString);

            this.context.CreatePlainHttpContext();

            var loggerFactory = new NullLoggerFactory();

            sut = new ResponseMiddleware(RequestDelegate, loggerFactory);
        }

        [Test]
        public void When_invoke_async_And_content_empty_Then_does_not_throw_exception()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.Created);

            var plainHttpContext = this.context.GetPlainHttpContext();
            plainHttpContext.SetResponse(response);

            //Act
            //Assert
            Assert.DoesNotThrowAsync(() => this.sut.InvokeAsync(this.context));

            Assert.That(this.context.Response.StatusCode, Is.EqualTo(201));
            Assert.That(this.context.Response.ContentLength, Is.EqualTo(0));
        }

        [Test]
        public void When_invoke_async_And_string_content_Then_does_not_throw_exception()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Response Payload")
            };

            var plainHttpContext = this.context.GetPlainHttpContext();
            plainHttpContext.SetResponse(response);

            //Act
            //Assert
            Assert.DoesNotThrowAsync(() => this.sut.InvokeAsync(this.context));

            Assert.That(this.context.Response.StatusCode, Is.EqualTo(200));
            Assert.That(this.context.Response.ContentLength, Is.EqualTo(16));
        }

        public async Task RequestDelegate(HttpContext httpContext)
        {
            await Task.CompletedTask;
        }
    }
}