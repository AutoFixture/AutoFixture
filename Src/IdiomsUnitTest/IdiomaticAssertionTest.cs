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

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMembersCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var members = type.GetMembers().AsEnumerable();
            var observedMembers = new List<MemberInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMemberInfoVerify = observedMembers.Add };
            // Exercise system
            sut.Verify(members);
            // Verify outcome
            Assert.True(members.SequenceEqual(observedMembers));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoCorrectlyInvokesConstructorVerify(Type type)
        {
            // Fixture setup
            var member = type.GetConstructors().Cast<MemberInfo>().First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = c => mockVerified = c.Equals(member) };
            // Exercise system
            sut.Verify(member);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoCorrectlyInvokesMethodVerify(Type type)
        {
            // Fixture setup
            var member = type.GetMethods()
                .Except(type.GetProperties().SelectMany(p => p.GetAccessors()))
                .Cast<MemberInfo>()
                .First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfoVerify = m => mockVerified = m.Equals(member) };
            // Exercise system
            sut.Verify(member);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoCorrectlyInvokesPropertyVerify(Type type)
        {
            // Fixture setup
            var member = type.GetProperties().Cast<MemberInfo>().First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = p => mockVerified = p.Equals(member) };
            // Exercise system
            sut.Verify(member);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyConstructorInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var ctors = type.GetConstructors();
            var observedConstructors = new List<ConstructorInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = observedConstructors.Add };
            // Exercise system
            sut.Verify(ctors);
            // Verify outcome
            Assert.True(ctors.SequenceEqual(observedConstructors));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyConstructorInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var ctors = type.GetConstructors().AsEnumerable();
            var observedConstructors = new List<ConstructorInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = observedConstructors.Add };
            // Exercise system
            sut.Verify(ctors);
            // Verify outcome
            Assert.True(ctors.SequenceEqual(observedConstructors));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyConstructorInfoDoesNotThrow(Type type)
        {
            // Fixture setup
            var ctor = type.GetConstructors().First();
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(ctor));
            // Teardown
        }


        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMethodInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var methods = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors())).ToArray();
            var observedMethods = new List<MethodInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfoVerify = observedMethods.Add };
            // Exercise system
            sut.Verify(methods);
            // Verify outcome
            Assert.True(methods.SequenceEqual(observedMethods));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMethodInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var methods = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
            var observedMethods = new List<MethodInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfoVerify = observedMethods.Add };
            // Exercise system
            sut.Verify(methods);
            // Verify outcome
            Assert.True(methods.SequenceEqual(observedMethods));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMethodInfoDoesNotThrow(Type type)
        {
            // Fixture setup
            var method = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors())).First();
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(method));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyPropertyInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var properties = type.GetProperties();
            var observedProperties = new List<PropertyInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = observedProperties.Add };
            // Exercise system
            sut.Verify(properties);
            // Verify outcome
            Assert.True(properties.SequenceEqual(observedProperties));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyPropertyInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Fixture setup
            var properties = type.GetProperties().AsEnumerable();
            var observedProperties = new List<PropertyInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = observedProperties.Add };
            // Exercise system
            sut.Verify(properties);
            // Verify outcome
            Assert.True(properties.SequenceEqual(observedProperties));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyPropertyInfoDoesNotThrow(Type type)
        {
            // Fixture setup
            var property = type.GetProperties().First();
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(property));
            // Teardown
        }
    }
}
