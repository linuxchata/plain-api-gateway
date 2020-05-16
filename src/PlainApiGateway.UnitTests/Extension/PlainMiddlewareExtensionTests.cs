using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

using PlainApiGateway.Configuration;
using PlainApiGateway.Extension;
using PlainApiGateway.Middleware;

// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PossibleNullReferenceException
namespace PlainApiGateway.UnitTests.Extension
{
    [TestFixture]
    public class PlainMiddlewareExtensionTests
    {
        private const int RequestRedirectMiddlewareIndex = 1;

        private const int ResponseMiddlewareIndex = 3;

        private readonly List<string> requiredMiddlewares = new List<string>
        {
            typeof(RequestRedirectMiddleware).FullName,
            typeof(ResponseMiddleware).FullName,
        };

        private Mock<IServiceProvider> serviceProviderMock;

        [SetUp]
        public void SetUp()
        {
            this.serviceProviderMock = new Mock<IServiceProvider>();
        }

        [Test]
        public void When_use_plain_api_gateway_And_application_builder_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => PlainMiddlewareExtension.UsePlainApiGateway(null));
        }

        [Test]
        public void When_use_plain_api_gateway_And_middleware_configuration_null_Then_all_required_middlewares_are_registered()
        {
            //Arrange
            var applicationBuilder = new ApplicationBuilder(this.serviceProviderMock.Object);

            //Act
            PlainMiddlewareExtension.UsePlainApiGateway(applicationBuilder);

            //Assert
            Assert.That(applicationBuilder, Is.Not.Null);

            this.AssertRequiredMiddlewares(applicationBuilder);
        }

        [Test]
        public void When_use_plain_api_gateway_And_middleware_configuration_empty_Then_all_required_middlewares_are_registered()
        {
            //Arrange
            var applicationBuilder = new ApplicationBuilder(this.serviceProviderMock.Object);

            //Act
            PlainMiddlewareExtension.UsePlainApiGateway(applicationBuilder, new PlainMiddlewareConfiguration());

            //Assert
            Assert.That(applicationBuilder, Is.Not.Null);

            this.AssertRequiredMiddlewares(applicationBuilder);
        }

        [Test]
        public void When_use_plain_api_gateway_And_with_middleware_configuration_Then_all_required_middlewares_are_registered()
        {
            //Arrange
            var applicationBuilder = new ApplicationBuilder(this.serviceProviderMock.Object);

            var plainMiddlewareConfiguration = new PlainMiddlewareConfiguration
            {
                PreRequestMiddleware = (context, next) => next(),
                PreResponseMiddleware = (context, next) => next(),
            };

            //Act
            PlainMiddlewareExtension.UsePlainApiGateway(applicationBuilder, plainMiddlewareConfiguration);

            //Assert
            Assert.That(applicationBuilder, Is.Not.Null);

            var components = this.GetComponents(applicationBuilder);

            string requestRedirectMiddlewareTypeName = this.GetMiddlewareTypeName(components[RequestRedirectMiddlewareIndex]);
            Assert.That(requestRedirectMiddlewareTypeName, Is.EqualTo(typeof(RequestRedirectMiddleware).FullName));

            string responseMiddlewareTypeName = this.GetMiddlewareTypeName(components[ResponseMiddlewareIndex]);
            Assert.That(responseMiddlewareTypeName, Is.EqualTo(typeof(ResponseMiddleware).FullName));
        }

        private void AssertRequiredMiddlewares(ApplicationBuilder applicationBuilder)
        {
            var components = this.GetComponents(applicationBuilder);
            for (int i = 0; i < components.Count; i++)
            {
                string middlewareTypeName = this.GetMiddlewareTypeName(components[i]);

                Assert.That(middlewareTypeName, Is.EqualTo(this.requiredMiddlewares[i]));
            }
        }

        private List<Func<RequestDelegate, RequestDelegate>> GetComponents(ApplicationBuilder applicationBuilder)
        {
            var componentsField = typeof(ApplicationBuilder).GetField("_components", BindingFlags.NonPublic | BindingFlags.Instance);
            var components = (List<Func<RequestDelegate, RequestDelegate>>)componentsField.GetValue(applicationBuilder);
            return components;
        }

        private string GetMiddlewareTypeName(Func<RequestDelegate, RequestDelegate> component)
        {
            var middlewareField = component.Target.GetType().GetField("middleware");
            var middlewareFieldValue = middlewareField.GetValue(component.Target);
            string middlewareTypeName = ((Type)middlewareFieldValue).FullName;
            return middlewareTypeName;
        }
    }
}