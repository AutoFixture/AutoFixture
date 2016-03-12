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
                new AutoDataAttribute((IFixture)null));
            // Teardown
        }

        [Test]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingFixture();
            var sut = new AutoDataAttribute(expectedComposer);
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
#pragma warning disable 618
                new AutoDataAttribute((Type)null));
#pragma warning restore 618
            // Teardown
        }

        [Test]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(object)));
#pragma warning restore 618
            // Teardown
        }

        [Test]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
#pragma warning disable 618
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
#pragma warning restore 618
            // Teardown
        }

        [Test]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
#pragma warning disable 618
            var sut = new AutoDataAttribute(composerType);
#pragma warning restore 618
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
#pragma warning disable 618
            var sut = new AutoDataAttribute(composerType);
#pragma warning restore 618
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

            var sut = new AutoDataAttribute(composer);
            // Exercise system
            var result = sut.GetData(method);
            // Verify outcome
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
            // Teardown
        }
    }
}
