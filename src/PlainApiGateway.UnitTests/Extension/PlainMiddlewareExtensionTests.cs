using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Moq;

using NUnit.Framework;

using PlainApiGateway.Extension;
using PlainApiGateway.Middleware;

// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PossibleNullReferenceException
namespace PlainApiGateway.UnitTests.Extension
{
    [TestFixture]
    public class PlainMiddlewareExtensionTests
    {
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
        public void When_use_plain_api_gateway_Then_all_required_middlewares_are_registered()
        {
            //Arrange
            var applicationBuilder = new ApplicationBuilder(this.serviceProviderMock.Object);

            //Act
            PlainMiddlewareExtension.UsePlainApiGateway(applicationBuilder);

            //Assert
            Assert.That(applicationBuilder, Is.Not.Null);

            this.AssertMiddlewares(applicationBuilder);
        }

        private void AssertMiddlewares(ApplicationBuilder applicationBuilder)
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