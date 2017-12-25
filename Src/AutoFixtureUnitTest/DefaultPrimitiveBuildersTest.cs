using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest
{
    public class DefaultPrimitiveBuildersTest
    {
        [Fact]
        public void SutIsSpecimenBuilders()
        {
            // Arrange
            // Act
            var sut = new DefaultPrimitiveBuilders();
            // Assert
            Assert.IsAssignableFrom<IEnumerable<ISpecimenBuilder>>(sut);
        }

        [Fact]
        public void SutHasCorrectContents()
        {
            // Arrange
            var expectedBuilderTypes = new[]
                {
                    typeof(StringGenerator),
                    typeof(ConstrainedStringGenerator),
                    typeof(StringSeedRelay),
                    typeof(RandomNumericSequenceGenerator),
                    typeof(RandomCharSequenceGenerator),
                    typeof(UriGenerator),
                    typeof(UriSchemeGenerator),
                    typeof(RandomRangedNumberGenerator),
                    typeof(RegularExpressionGenerator),
                    typeof(RandomDateTimeSequenceGenerator),
                    typeof(BooleanSwitch),
                    typeof(GuidGenerator),
                    typeof(TypeGenerator),
                    typeof(DelegateGenerator),
                    typeof(TaskGenerator),
                    typeof(IntPtrGuard),
#if SYSTEM_NET_MAIL
                    typeof(MailAddressGenerator),
#endif
                    typeof(EmailAddressLocalPartGenerator),
                    typeof(DomainNameGenerator)
                 };
            // Act
            var sut = new DefaultPrimitiveBuilders();
            // Assert
            Assert.True(expectedBuilderTypes.SequenceEqual(sut.Select(b => b.GetType())));
        }

        [Fact]
        public void NonGenericEnumeratorMatchesGenericEnumerator()
        {
            // Arrange
            var sut = new DefaultPrimitiveBuilders();
            // Act
            IEnumerable result = sut;
            // Assert
            Assert.True(sut.Select(b => b.GetType()).SequenceEqual(result.Cast<object>().Select(o => o.GetType())));
        }

        [Fact]
        public void StringGeneratorHasFactoryThatCreatesCorrectType()
        {
            // Arrange
            var sut = new DefaultPrimitiveBuilders();
            // Act
            var result = sut.OfType<StringGenerator>().Single();
            // Assert
            Assert.IsAssignableFrom<Guid>(result.Factory());
        }

        [Fact]
        public void StringGeneratorFactoryReturnsNewInstancesForEachCall()
        {
            // Arrange
            var sut = new DefaultPrimitiveBuilders();
            // Act
            var result = sut.OfType<StringGenerator>().Single();
            // Assert
            Assert.NotEqual(result.Factory(), result.Factory());
        }
    }
}
