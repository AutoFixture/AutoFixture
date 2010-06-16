using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixtureUnitTest.Dsl;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest
{
    // Regression tests of obsolete API until it is removed completely
#pragma warning disable 618

    public class FactoryComposerTest
    {
        [Fact]
        public void WithConstructorCorrectlyDelegates()
        {
            // Fixture setup
            Func<object> factory = () => new object();
            var expectedComposer = new DelegatingComposer();
            var composer = new DelegatingComposer { OnFromFactory = f => f == factory ? expectedComposer : new DelegatingComposer() };
            // Exercise system
            IPostprocessComposer<object> result = composer.WithConstructor(factory);
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void WithSingleInputConstructorCorrectlyDelegates()
        {
            // Fixture setup
            Func<object, object> factory = x => new object();
            var expectedComposer = new DelegatingComposer();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(factory) ? expectedComposer : new DelegatingComposer() };
            // Exercise system
            IPostprocessComposer<object> result = composer.WithConstructor(factory);
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void WithDoubleInputConstructorCorrectlyDelegates()
        {
            // Fixture setup
            Func<object, object, object> factory = (x, y) => new object();
            var expectedComposer = new DelegatingComposer();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(factory) ? expectedComposer : new DelegatingComposer() };
            // Exercise system
            IPostprocessComposer<object> result = composer.WithConstructor(factory);
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void WithTripeInputConstructorCorrectlyDelegates()
        {
            // Fixture setup
            Func<object, object, object, object> factory = (x, y, z) => new object();
            var expectedComposer = new DelegatingComposer();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(factory) ? expectedComposer : new DelegatingComposer() };
            // Exercise system
            IPostprocessComposer<object> result = composer.WithConstructor(factory);
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void WithQuadrupleInputConstructorCorrectlyDelegates()
        {
            // Fixture setup
            Func<object, object, object, object, object> factory = (x, y, z, æ) => new object();
            var expectedComposer = new DelegatingComposer();
            var composer = new DelegatingComposer { OnFromOverloadeFactory = f => f.Equals(factory) ? expectedComposer : new DelegatingComposer() };
            // Exercise system
            IPostprocessComposer<object> result = composer.WithConstructor(factory);
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }
    }

#pragma warning restore 618
}
