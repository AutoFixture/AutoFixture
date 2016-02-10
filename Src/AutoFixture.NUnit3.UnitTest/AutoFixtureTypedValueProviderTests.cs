using System;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoFixtureTypedValueProviderTests
    {
        [Test]
        public void If_Fixture_is_null_Then_throws()
        {
            Assert.Throws<ArgumentNullException>(() => new AutoFixtureTypedValueProvider(null));
        }

        [Repeat(10)]
        [Test]
        public void When_CreateFrozenValue_is_called_twice_Will_return_same_value()
        {
            // Arrange
            var autoFixtureTypedValueProvider = new AutoFixtureTypedValueProvider(new Fixture());

            // Act
            var val1 = autoFixtureTypedValueProvider.CreateFrozenValue(typeof (DateTime));
            var val2 = autoFixtureTypedValueProvider.CreateFrozenValue(typeof (DateTime));

            // Assert
            Assert.That(val1, Is.EqualTo(val2));
        }

        [Repeat(10)]
        [Theory]
        public void When_CreateValue_is_called_twice_Will_return_different_values()
        {
            // Arrange
            var autoFixtureTypedValueProvider = new AutoFixtureTypedValueProvider(new Fixture());

            // Act
            var val1 = autoFixtureTypedValueProvider.CreateValue(typeof (DateTime));
            var val2 = autoFixtureTypedValueProvider.CreateValue(typeof (DateTime));

            // Assert
            Assert.That(val1, Is.Not.EqualTo(val2));
        }
    }
}
