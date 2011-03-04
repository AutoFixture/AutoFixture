using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeMemberContextTest
    {
        [Fact]
        public void SutIsMethodContext()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeMemberContext();
            // Verify outcome
            Assert.IsAssignableFrom<IMemberContext>(sut);
            // Teardown
        }

        [Fact]
        public void MethodContextsIsCorrectWhenInitializedByArray()
        {
            // Fixture setup
            var expectedMethodContexts = new[] { new DelegatingMethodContext(), new DelegatingMethodContext(), new DelegatingMethodContext() };
            var sut = new CompositeMemberContext(expectedMethodContexts);
            // Exercise system
            IEnumerable<IMemberContext> result = sut.MemberContexts;
            // Verify outcome
            Assert.True(expectedMethodContexts.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMemberContext((IMemberContext[])null));
            // Teardown
        }

        [Fact]
        public void MethodContextIsCorrectWhenInitializedByEnumerable()
        {
            // Fixture setup
            var expectedMethodContexts = new[] { new DelegatingMemberContext(), new DelegatingMemberContext(), new DelegatingMemberContext() }.AsEnumerable().Cast<IMemberContext>();
            var sut = new CompositeMemberContext(expectedMethodContexts);
            // Exercise system
            var result = sut.MemberContexts;
            // Verify outcome
            Assert.True(expectedMethodContexts.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeMemberContext((IEnumerable<IMemberContext>)null));
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
                    new DelegatingMemberContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}},
                    new DelegatingMemberContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}},
                    new DelegatingMemberContext{ OnVerifyBoundaries = c => { if (c == convention){ invocationCount++;}}}
                };
            var sut = new CompositeMemberContext(contexts);
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
            var sut = new CompositeMemberContext();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.VerifyBoundaries(null));
            // Teardown
        }
    }
}
