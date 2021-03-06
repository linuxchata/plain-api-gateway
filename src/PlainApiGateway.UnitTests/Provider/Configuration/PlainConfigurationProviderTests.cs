﻿using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;

using NUnit.Framework;

using PlainApiGateway.Provider.Configuration;

namespace PlainApiGateway.UnitTests.Provider.Configuration
{
    [TestFixture]
    public class PlainConfigurationProviderTests
    {
        private const ushort DefaultTimeoutInSeconds = 30;

        private const string ConfigurationFileName = "plainsettings.json";

        private PlainConfigurationProvider sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.sut = new PlainConfigurationProvider();
        }

        [Test]
        public void When_read_And_configuration_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.sut.Read(null));
        }

        [Test]
        public void When_read_And_configuration_without_routes_Then_throws_argument_null_exception()
        {
            //Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => this.sut.Read(configuration));
        }

        [Test]
        public void When_read_And_configuration_valid_without_timeout_Then_return_plain_configuration()
        {
            //Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigurationFileName)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "TimeoutInSeconds", null }
                })
                .Build();

            //Act
            var result = this.sut.Read(configuration);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Routes, Is.Not.Null);
            Assert.That(result.Routes.Count, Is.EqualTo(1));
            Assert.That(result.Routes[0].Source, Is.Not.Null);
            Assert.That(result.Routes[0].Target, Is.Not.Null);
            Assert.That(result.TimeoutInSeconds, Is.EqualTo(DefaultTimeoutInSeconds));
        }

        [Test]
        public void When_read_And_configuration_valid_Then_return_plain_configuration()
        {
            //Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigurationFileName)
                .Build();

            //Act
            var result = this.sut.Read(configuration);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Routes, Is.Not.Null);
            Assert.That(result.Routes.Count, Is.EqualTo(1));
            Assert.That(result.Routes[0].Source, Is.Not.Null);
            Assert.That(result.Routes[0].Target, Is.Not.Null);
            Assert.That(result.TimeoutInSeconds, Is.EqualTo(5));
        }
    }
}