using System;
using System.Linq;
using Ploeh.AutoFixture.NUnit2.Addins;
using NUnit.Framework;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeTest
    {
        [Test]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataAttribute();
            // Verify outcome
            Assert.IsInstanceOf<DataAttribute>(sut);
            // Teardown
        }

        [Test]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            // Exercise system
            IFixture result = sut.Fixture;
            // Verify outcome
            Assert.IsAssignableFrom<Fixture>(result);
            // Teardown
        }

        [Test]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new DerivedAutoDataAttribute((IFixture)null));
            // Teardown
        }

        [Test]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingFixture();
            var sut = new DerivedAutoDataAttribute(expectedComposer);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.AreEqual(expectedComposer, result);
            // Teardown
        }

        [Test]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoDataAttribute((Type)null));
            // Teardown
        }

        [Test]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new AutoDataAttribute(typeof(object)));
            // Teardown
        }

        [Test]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
            // Teardown
        }

        [Test]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.IsAssignableFrom(composerType, result);
            // Teardown
        }

        [Test]
        public void FixtureTypeIsCorrect()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.FixtureType;
            // Verify outcome
            Assert.AreEqual(composerType, result);
            // Teardown
        }

        [Test]
        public void GetArgumentsWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null));
            // Teardown
        }

        [Test]
        public void GetArgumentsReturnsCorrectResult()
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var parameters = method.GetParameters();
            
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                    {
                        Assert.AreEqual(parameters.Single(), r);
                        Assert.NotNull(c);
                        return expectedResult;
                    }
            };
            var composer = new DelegatingFixture { OnCreate = builder.OnCreate };

            var sut = new DerivedAutoDataAttribute(composer);
            // Exercise system
            var result = sut.GetData(method);
            // Verify outcome
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
            // Teardown
        }

        private class DerivedAutoDataAttribute : AutoDataAttribute
        {
            public DerivedAutoDataAttribute(IFixture fixture)
               : base(fixture)
            {
            }
        }
    }
}
