using System;
using System.Net.Http;

using Microsoft.AspNetCore.Http;

using NUnit.Framework;

using PlainApiGateway.Handler;

namespace PlainApiGateway.UnitTests.Handler
{
    [TestFixture]
    public class ErrorHandlerTests
    {
        private ErrorHandler sut;

        [SetUp]
        public void SetUp()
        {
            var nullLoggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();

            this.sut = new ErrorHandler(nullLoggerFactory);
        }

        [Test]
        public void When_set_route_not_found_error_response_And_http_context_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.sut.SetRouteNotFoundErrorResponse(null));
        }

        [Test]
        public void When_set_route_not_found_error_response_Then_status_code_set_to_not_found()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = HttpMethod.Get.Method;
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("host");
            httpContext.Request.Path = "/path/";

            //Act
            this.sut.SetRouteNotFoundErrorResponse(httpContext);

            //Assert
            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}