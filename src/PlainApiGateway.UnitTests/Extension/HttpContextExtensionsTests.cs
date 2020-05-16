using System;

using Microsoft.AspNetCore.Http;

using NUnit.Framework;

using PlainApiGateway.Domain.Http;
using PlainApiGateway.Extension;

// ReSharper disable InvokeAsExtensionMethod
namespace PlainApiGateway.UnitTests.Extension
{
    [TestFixture]
    public class HttpContextExtensionsTests
    {
        [Test]
        public void When_create_plain_http_context_And_http_context_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => HttpContextExtensions.CreatePlainHttpContext(null));
        }

        [Test]
        public void When_create_plain_http_context_And_http_context_not_empty_Then_throws_invalid_operation_exception()
        {
            //Arrange
            var plainHttpContext = new PlainHttpContext();
            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Features.Set(plainHttpContext);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => HttpContextExtensions.CreatePlainHttpContext(defaultHttpContext));
        }

        [Test]
        public void When_create_plain_http_context_And_http_context_empty_Then_returns_plain_http_context()
        {
            //Act
            var result = HttpContextExtensions.CreatePlainHttpContext(new DefaultHttpContext());

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void When_get_plain_http_context_And_http_context_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => HttpContextExtensions.GetPlainHttpContext(null));
        }

        [Test]
        public void When_get_plain_http_context_And_http_context_empty_Then_throws_invalid_operation_exception()
        {
            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => HttpContextExtensions.GetPlainHttpContext(new DefaultHttpContext()));
        }

        [Test]
        public void When_get_plain_http_context_And_http_context_not_empty_Then_returns_plain_http_context()
        {
            //Arrange
            var plainHttpContext = new PlainHttpContext();
            var defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Features.Set(plainHttpContext);

            //Act
            var result = HttpContextExtensions.GetPlainHttpContext(defaultHttpContext);

            //Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}