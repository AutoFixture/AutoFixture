using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.NUnit2.Addins;
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

            var sut = new AutoDataAttribute(composer);
            // Exercise system
            var result = sut.GetData(method);
            // Verify outcome
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
            // Teardown
        }

        [Test]
        public void GetDataReturnsValuesSuppliedByTestDataProvider()
        {
            // Fixture setup
            var arguments = new[] { new object(), new object() };
            var provider = new DelegatingTestDataProvider { OnGetData = m => arguments };
            var sut = new TestableAutoDataAttribute(new DelegatingFixture()) { OnCreateDataProvider = () => provider };
            // Excercise system
            IEnumerable<object[]> result = sut.GetData(new Action<object>(ParameterizedMethod).Method);
            // Verify outcome
            Assert.AreEqual(arguments, result.Single());
            // Teardown
        }

        [Test]
        public void CreateDataProviderReturnsTestDataProviderConstructedWithGivenFixture()
        {
            // Fixture setup
            bool fixtureInvoked = false;
            var fixture = new DelegatingFixture { OnCreate = (request, context) => fixtureInvoked = true };
            var sut = new TestableAutoDataAttribute(fixture);
            // Excercise system
            ITestDataProvider provider = sut.TestableCreateDataProvider();
            // Verify outcome
            provider.GetData(new Action<object>(ParameterizedMethod).Method);
            Assert.IsTrue(fixtureInvoked);
            // Teardown
        }

        private static void ParameterizedMethod(object parameter)
        {
        }

        private class TestableAutoDataAttribute : AutoDataAttribute
        {
            public Func<ITestDataProvider> OnCreateDataProvider;

            public TestableAutoDataAttribute(IFixture fixture)
                : base(fixture)
            {
                this.OnCreateDataProvider = base.CreateDataProvider;
            }

            public ITestDataProvider TestableCreateDataProvider()
            {
                return base.CreateDataProvider();
            }

            protected override ITestDataProvider CreateDataProvider()
            {
                return this.OnCreateDataProvider();
            }
        }
    }
}
