using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class AutoDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<DataAttribute>(sut);
            // Teardown
        }

        [Fact]
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

        [Fact]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoDataAttribute((IFixture)null));
            // Teardown
        }

        [Fact]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingFixture();
            var sut = new AutoDataAttribute(expectedComposer);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new AutoDataAttribute((Type)null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new AutoDataAttribute(typeof(object)));
            // Teardown
        }

        [Fact]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() =>
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
            // Teardown
        }

        [Fact]
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

        [Fact]
        public void FixtureTypeIsCorrect()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.FixtureType;
            // Verify outcome
            Assert.Equal(composerType, result);
            // Teardown
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null, dummyTypes));
            // Teardown
        }

        [Fact]
        public void GetDataReturnsCorrectResult()
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                    {
                        Assert.Equal(parameters.Single(), r);
                        Assert.NotNull(c);
                        return expectedResult;
                    }
            };
            var composer = new DelegatingFixture { OnCreate = builder.OnCreate };

            var sut = new AutoDataAttribute(composer);
            // Exercise system
            var result = sut.GetData(method, parameterTypes);
            // Verify outcome
            Assert.True(new[] { expectedResult }.SequenceEqual(result.Single()));
            // Teardown
        }

        [Fact]
        public void GetDataReturnsValuesSuppliedByTestDataProvider()
        {
            // Fixture setup
            var arguments = new[] { new object(), new object() };
            var provider = new DelegatingTestDataProvider { OnGetData = m => arguments };
            var sut = new TestableAutoDataAttribute(new DelegatingFixture()) { OnCreateDataProvider = () => provider };
            // Excercise system
            IEnumerable<object[]> result = sut.GetData(new Action<object>(ParameterizedMethod).Method, new Type[0]);
            // Verify outcome
            Assert.Equal(arguments, result.Single());
            // Teardown
        }

        [Fact]
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
            Assert.True(fixtureInvoked);
            // Teardown
        }

        private static void ParameterizedMethod(object parameter)
        {
        }

        private class TestableAutoDataAttribute : AutoDataAttribute
        {
            public Func<ITestDataProvider> OnCreateDataProvider;

            public TestableAutoDataAttribute(IFixture fixture) : base(fixture)
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
