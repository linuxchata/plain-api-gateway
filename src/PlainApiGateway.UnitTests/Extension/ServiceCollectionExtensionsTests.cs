using System;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using PlainApiGateway.Extension;

// ReSharper disable InvokeAsExtensionMethod
namespace PlainApiGateway.UnitTests.Extension
{
    [TestFixture]
    public class ServiceCollectionExtensionsTests
    {
        private const string ConfigurationFileName = "plainsettings.json";

        [Test]
        public void When_add_plain_api_gateway_And_service_collection_null_Then_throws_argument_null_exception()
        {
            //Arrange
            var configuration = this.GetConfiguration();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddPlainApiGateway(null, configuration));
        }

        [Test]
        public void When_add_plain_api_gateway_And_configuration_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddPlainApiGateway(new ServiceCollection(), null));
        }

        [Test]
        public void When_add_plain_api_gateway_Then_does_not_throw_exception()
        {
            //Arrange
            var configuration = this.GetConfiguration();

            //Act
            //Assert
            Assert.DoesNotThrow(() => ServiceCollectionExtensions.AddPlainApiGateway(new ServiceCollection(), configuration));
        }

        private IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigurationFileName)
                .Build();
        }
    }
}