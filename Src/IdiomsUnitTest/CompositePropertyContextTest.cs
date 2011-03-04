using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositePropertyContextTest
    {
        [Fact]
        public void SutIsMethodContext()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositePropertyContext();
            // Verify outcome
            Assert.IsAssignableFrom<IPropertyContext>(sut);
            // Teardown
        }

        [Fact]
        public void MethodContextsIsCorrectWhenInitializedByArray()
        {
            // Fixture setup
            var expectedPropertyContexts = new[] { new DelegatingPropertyContext(), new DelegatingPropertyContext(), new DelegatingPropertyContext() };
            var sut = new CompositePropertyContext(expectedPropertyContexts);
            // Exercise system
            IEnumerable<IPropertyContext> result = sut.PropertyContexts;
            // Verify outcome
            Assert.True(expectedPropertyContexts.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositePropertyContext((IPropertyContext[])null));
            // Teardown
        }

        [Fact]
        public void MethodContextIsCorrectWhenInitializedByEnumerable()
        {
            // Fixture setup
            var expectedPropertyContexts = new[] { new DelegatingPropertyContext(), new DelegatingPropertyContext(), new DelegatingPropertyContext() }.AsEnumerable().Cast<IPropertyContext>();
            var sut = new CompositePropertyContext(expectedPropertyContexts);
            // Exercise system
            var result = sut.PropertyContexts;
            // Verify outcome
            Assert.True(expectedPropertyContexts.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositePropertyContext((IEnumerable<IPropertyContext>)null));
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesVerifiesBoundariesForAllContexts()
        {
            // Fixture setup
            var convention = new DelegatingBoundaryConvention();
            var invocationCount = 0;
            var contexts = new[]
                {
                    new DelegatingPropertyContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}},
                    new DelegatingPropertyContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}},
                    new DelegatingPropertyContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}}
                };
            var sut = new CompositePropertyContext(contexts);
            // Exercise system
            sut.VerifyBoundaries(convention);
            // Verify outcome
            Assert.Equal(contexts.Length, invocationCount);
            // Teardown
        }

        [Fact]
        public void VerifyBoundariesWithNullConventionThrows()
        {
            // Fixture setup
            var sut = new CompositePropertyContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.VerifyBoundaries(null));
            // Teardown
        }

        [Fact]
        public void VerifyWritableVerifiesWritableForAllContexts()
        {
            // Fixture setup
            var invocations = new List<int>();
            var contexts = new[]
                {
                    new DelegatingPropertyContext{ OnVerifyWritable = () => invocations.Add(1) },
                    new DelegatingPropertyContext{ OnVerifyWritable = () => invocations.Add(2) },
                    new DelegatingPropertyContext{ OnVerifyWritable = () => invocations.Add(3) }
                };
            var sut = new CompositePropertyContext(contexts);
            // Exercise system
            sut.VerifyWritable();
            // Verify outcome
            Assert.True(Enumerable.Range(1, 3).SequenceEqual(invocations));
            // Teardown
        }
    }
}
