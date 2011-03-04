using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeMethodContextTest
    {
        [Fact]
        public void SutIsMethodContext()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeMethodContext();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodContext>(sut);
            // Teardown
        }

        [Fact]
        public void MemberContextsIsCorrectWhenInitializedByArray()
        {
            // Fixture setup
            var expectedMemberContexts = new[] { new DelegatingMethodContext(), new DelegatingMethodContext(), new DelegatingMethodContext() };
            var sut = new CompositeMethodContext(expectedMemberContexts);
            // Exercise system
            IEnumerable<IMethodContext> result = sut.MethodContexts;
            // Verify outcome
            Assert.True(expectedMemberContexts.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMethodContext((IMethodContext[])null));
            // Teardown
        }

        [Fact]
        public void MemberContextsIsCorrectWhenInitializedByEnumerable()
        {
            // Fixture setup
            var expectedMemberContexts = new[] { new DelegatingMethodContext(), new DelegatingMethodContext(), new DelegatingMethodContext() }.AsEnumerable().Cast<IMethodContext>();
            var sut = new CompositeMethodContext(expectedMemberContexts);
            // Exercise system
            var result = sut.MethodContexts;
            // Verify outcome
            Assert.True(expectedMemberContexts.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMethodContext((IEnumerable<IMethodContext>)null));
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
                    new DelegatingMethodContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}},
                    new DelegatingMethodContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}},
                    new DelegatingMethodContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}}
                };
            var sut = new CompositeMethodContext(contexts);
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
            var sut = new CompositeMethodContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.VerifyBoundaries(null));
            // Teardown
        }
    }
}
