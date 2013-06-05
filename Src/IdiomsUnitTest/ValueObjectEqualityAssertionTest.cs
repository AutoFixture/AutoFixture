using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ValueObjectEqualityAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new ValueObjectEqualityAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system
            ISpecimenBuilder result = sut.Fixture;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new ValueObjectEqualityAssertion(null));
            // Teardown
        }

        [Fact]
        public void ProperValueObjectWithEqualsOverriden()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeof (ValueObject)));
            // Teardown
        }

        [Fact]
        public void ValueObjectWithoutEqualsThrowsException()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.Throws<ValueObjectEqualityException>(() =>
                                                        sut.Verify(typeof (ValueObjectWithoutEquals)));

            // Teardown
        }

        [Fact]
        public void MutableValueObjectWithEqualsDoesntThrow()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeof(MutableValueObject)));

            // Teardown
        }

        [Fact]
        public void ValueObjectWithBadEqualsDoesThrow()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.Throws<ValueObjectEqualityException>(() => sut.Verify(typeof(ValueObjectWithBadEquals)));

            // Teardown
        }

        [Fact] 
        public void ValueObjectWithBadIEquatableDoesThrow()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.Throws<ValueObjectEqualityException>(() => sut.Verify(typeof(ValueObjectWithBadIEquatableImplementation)));

            // Teardown
        }

        [Fact]
        public void ValueObjectWithIEquatableDoesNotThrow()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Verify(typeof(ValueObjectWithIEquatableImplemented)));

            // Teardown
        }

        [Fact]
        public void ValueObjectWithoutConstructorAndBadEqualsThrows()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new ValueObjectEqualityAssertion(expectedComposer);
            // Exercise system and verify outcome
            Assert.Throws<ValueObjectEqualityException>(
                () => sut.Verify(typeof (ValueObjectWithoutConstructorAndBadEquals)));
            // Teardown
        } 
    }
}