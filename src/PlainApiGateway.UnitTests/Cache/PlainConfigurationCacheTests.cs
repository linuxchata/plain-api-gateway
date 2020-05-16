using System;

using NUnit.Framework;

using PlainApiGateway.Cache;
using PlainApiGateway.Configuration;

namespace PlainApiGateway.UnitTests.Cache
{
    [TestFixture]
    public class PlainConfigurationCacheTests
    {
        private PlainConfigurationCache sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new PlainConfigurationCache();
        }

        [Test]
        public void When_get_And_cache_empty_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.sut.Get());
        }

        [Test]
        public void When_get_And_cache_not_empty_Then_return_plain_configuration()
        {
            //Arrange
            this.sut.Add(new PlainConfiguration());

            //Act
            var result = this.sut.Get();

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void When_add_And_plain_configuration_null_Then_throws_argument_null_exception()
        {
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.sut.Add(null));
        }

        [Test]
        public void When_add_And_plain_configuration_not_null_Then_does_not_throw_exception()
        {
            //Act
            //Assert
            Assert.DoesNotThrow(() => this.sut.Add(new PlainConfiguration()));
        }
    }
}