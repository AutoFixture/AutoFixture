using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class IdiomaticAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.True(typeof(IIdiomaticAssertion).IsAssignableFrom(typeof(IdiomaticAssertion)));
            // Teardown
        }

        [Fact]
        public void TestableSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingIdiomaticAssertion();
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void VerifyAssemblyCorrectlyInvokesNextVerify()
        {
            // Fixture setup
            var assembly = this.GetType().Assembly;
            var expectedTypes = assembly.GetExportedTypes();

            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnTypeArrayVerify = t => mockVerified = expectedTypes.SequenceEqual(t) };
            // Exercise system
            sut.Verify(assembly);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void VerifyTypeArrayCorrectlyInvokesNextVerify()
        {
            // Fixture setup
            var types = new[] { typeof(string), typeof(int), typeof(Version) };

            var observedTypes = new List<Type>();
            var sut = new DelegatingIdiomaticAssertion { OnTypeVerify = observedTypes.Add };
            // Exercise system
            sut.Verify(types);
            // Verify outcome
            Assert.True(types.SequenceEqual(observedTypes));
            // Teardown
        }

        [Fact]
        public void VerifyTypesCorrectlyInvokesNextVerify()
        {
            // Fixture setup
            var types = new[] { typeof(string), typeof(int), typeof(Version) }.AsEnumerable();

            var observedTypes = new List<Type>();
            var sut = new DelegatingIdiomaticAssertion { OnTypeVerify = observedTypes.Add };
            // Exercise system
            sut.Verify(types);
            // Verify outcome
            Assert.True(types.SequenceEqual(observedTypes));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]        
        [InlineData(typeof(Version))]
        public void VerifyTypeCorrectlyInvokesConstructorsVerify(Type type)
        {
            // Fixture setup
            var expectedCtors = type.GetConstructors();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoArrayVerify = c => mockVerified = expectedCtors.SequenceEqual(c) };
            // Exercise system
            sut.Verify(type);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyTypeCorrectlyInvokesMethodsVerify(Type type)
        {
            // Fixture setup
            var expectedMethods = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfosVerify = m => mockVerified = expectedMethods.SequenceEqual(m) };
            // Exercise system
            sut.Verify(type);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyTypeCorrectlyInvokesPropertiesVerify(Type type)
        {
            // Fixture setup
            var expectedProperties = type.GetProperties();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoArrayVerify = p => mockVerified = expectedProperties.SequenceEqual(p) };
            // Exercise system
            sut.Verify(type);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var members = type.GetMembers();
            var observedMembers = new List<MemberInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMemberInfoVerify = observedMembers.Add };
            // Exercise system
            sut.Verify(members);
            // Verify outcome
            Assert.True(members.SequenceEqual(observedMembers));
            // Teardown
        }
    }
}
