using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Idioms;
using Xunit;
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
        public void VerifyNullAssemblyArrayThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Assembly[])null));
            // Teardown
        }

        [Fact]
        public void VerifyAssemblyArrayCorrectlyInvokesNextVerify()
        {
            // Fixture setup
            var assemblies = new[] { this.GetType().Assembly, typeof(IdiomaticAssertion).GetType().Assembly, typeof(Fixture).GetType().Assembly };

            var observedAssemblies = new List<Assembly>();
            var sut = new DelegatingIdiomaticAssertion { OnAssemblyVerify = observedAssemblies.Add };
            // Exercise system
            sut.Verify(assemblies);
            // Verify outcome
            Assert.True(assemblies.SequenceEqual(observedAssemblies));
            // Teardown
        }

        [Fact]
        public void VerifyAssembliesCorrectlyInvokesNextVerify()
        {
            // Fixture setup
            var assemblies = new[] { typeof(IdiomaticAssertion).GetType().Assembly, this.GetType().Assembly, typeof(Fixture).GetType().Assembly }.AsEnumerable();

            var observedAssemblies = new List<Assembly>();
            var sut = new DelegatingIdiomaticAssertion { OnAssemblyVerify = observedAssemblies.Add };
            // Exercise system
            sut.Verify(assemblies);
            // Verify outcome
            Assert.True(assemblies.SequenceEqual(observedAssemblies));
            // Teardown
        }

        [Fact]
        public void VerifyNullAssemblyThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Assembly)null));
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
        public void VerifyNullTypeArrayThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Type[])null));
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

        [Fact]
        public void VerifyNullTypeThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Type)null));
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
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoArrayVerify = c => mockVerified = expectedCtors.IsEquivalentTo(c) };
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
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfosVerify = m => mockVerified = expectedMethods.IsEquivalentTo(m) };
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
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoArrayVerify = p => mockVerified = expectedProperties.IsEquivalentTo(p) };
            // Exercise system
            sut.Verify(type);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void VerifyNullMemberInfoArrayThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MemberInfo[])null));
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

        [Fact]
        public void VerifyNullConstructorInfoArrayThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((ConstructorInfo[])null));
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

        [Fact]
        public void VerifyNullMethodInfoArrayThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo[])null));
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

        [Fact]
        public void VerifyNullPropertyInfoArrayThrows()
        {
            // Fixture setup
            var sut = new DelegatingIdiomaticAssertion();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo[])null));
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
