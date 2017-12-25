using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Idioms;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class IdiomaticAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(typeof(IIdiomaticAssertion).IsAssignableFrom(typeof(IdiomaticAssertion)));
        }

        [Fact]
        public void TestableSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingIdiomaticAssertion();
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void VerifyNullAssemblyArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Assembly[])null));
        }

        [Fact]
        public void VerifyAssemblyArrayCorrectlyInvokesNextVerify()
        {
            // Arrange
            var assemblies = new[] { this.GetType().Assembly, typeof(IdiomaticAssertion).GetType().Assembly, typeof(Fixture).GetType().Assembly };

            var observedAssemblies = new List<Assembly>();
            var sut = new DelegatingIdiomaticAssertion { OnAssemblyVerify = observedAssemblies.Add };
            // Act
            sut.Verify(assemblies);
            // Assert
            Assert.True(assemblies.SequenceEqual(observedAssemblies));
        }

        [Fact]
        public void VerifyAssembliesCorrectlyInvokesNextVerify()
        {
            // Arrange
            var assemblies = new[] { typeof(IdiomaticAssertion).GetType().Assembly, this.GetType().Assembly, typeof(Fixture).GetType().Assembly }.AsEnumerable();

            var observedAssemblies = new List<Assembly>();
            var sut = new DelegatingIdiomaticAssertion { OnAssemblyVerify = observedAssemblies.Add };
            // Act
            sut.Verify(assemblies);
            // Assert
            Assert.True(assemblies.SequenceEqual(observedAssemblies));
        }

        [Fact]
        public void VerifyNullAssemblyThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Assembly)null));
        }

        [Fact]
        public void VerifyAssemblyCorrectlyInvokesNextVerify()
        {
            // Arrange
            var assembly = this.GetType().Assembly;
            var expectedTypes = assembly.GetExportedTypes();

            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnTypeArrayVerify = t => mockVerified = expectedTypes.SequenceEqual(t) };
            // Act
            sut.Verify(assembly);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void VerifyNullTypeArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Type[])null));
        }

        [Fact]
        public void VerifyTypeArrayCorrectlyInvokesNextVerify()
        {
            // Arrange
            var types = new[] { typeof(string), typeof(int), typeof(Version) };

            var observedTypes = new List<Type>();
            var sut = new DelegatingIdiomaticAssertion { OnTypeVerify = observedTypes.Add };
            // Act
            sut.Verify(types);
            // Assert
            Assert.True(types.SequenceEqual(observedTypes));
        }

        [Fact]
        public void VerifyTypesCorrectlyInvokesNextVerify()
        {
            // Arrange
            var types = new[] { typeof(string), typeof(int), typeof(Version) }.AsEnumerable();

            var observedTypes = new List<Type>();
            var sut = new DelegatingIdiomaticAssertion { OnTypeVerify = observedTypes.Add };
            // Act
            sut.Verify(types);
            // Assert
            Assert.True(types.SequenceEqual(observedTypes));
        }

        [Fact]
        public void VerifyNullTypeThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((Type)null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]        
        [InlineData(typeof(Version))]
        public void VerifyTypeCorrectlyInvokesConstructorsVerify(Type type)
        {
            // Arrange
            var expectedCtors = type.GetConstructors();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoArrayVerify = c => mockVerified = expectedCtors.IsEquivalentTo(c) };
            // Act
            sut.Verify(type);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyTypeCorrectlyInvokesMethodsVerify(Type type)
        {
            // Arrange
            var expectedMethods = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfosVerify = m => mockVerified = expectedMethods.IsEquivalentTo(m) };
            // Act
            sut.Verify(type);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void VerifyStaticTypeCorrectlyInvokesMethodsVerify()
        {
            // Arrange
            Type type = typeof(UnguardedStaticMethodOnStaticTypeHost);
            var expectedMethods = new[] { type.GetMethod("Method")};
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion
            {
                OnMethodInfosVerify = m => mockVerified = expectedMethods.IsEquivalentTo(m)
            };
            // Act
            sut.Verify(type);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyTypeCorrectlyInvokesPropertiesVerify(Type type)
        {
            // Arrange
            var expectedProperties = type.GetProperties();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoArrayVerify = p => mockVerified = expectedProperties.IsEquivalentTo(p) };
            // Act
            sut.Verify(type);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(TypeWithFields))]
        public void VerifyTypeCorrectlyInvokesFieldsVerify(Type type)
        {
            // Arrange
            var expectedFields = type.GetFields();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnFieldInfoArrayVerify = p => mockVerified = expectedFields.IsEquivalentTo(p) };
            // Act
            sut.Verify(type);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void VerifyNullMemberInfoArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MemberInfo[])null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var members = type.GetMembers();
            var observedMembers = new List<MemberInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMemberInfoVerify = observedMembers.Add };
            // Act
            sut.Verify(members);
            // Assert
            Assert.True(members.SequenceEqual(observedMembers));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMembersCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var members = type.GetMembers().AsEnumerable();
            var observedMembers = new List<MemberInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMemberInfoVerify = observedMembers.Add };
            // Act
            sut.Verify(members);
            // Assert
            Assert.True(members.SequenceEqual(observedMembers));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoCorrectlyInvokesConstructorVerify(Type type)
        {
            // Arrange
            var member = type.GetConstructors().Cast<MemberInfo>().First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = c => mockVerified = c.Equals(member) };
            // Act
            sut.Verify(member);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoCorrectlyInvokesMethodVerify(Type type)
        {
            // Arrange
            var member = type.GetMethods()
                .Except(type.GetProperties().SelectMany(p => p.GetAccessors()))
                .Cast<MemberInfo>()
                .First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfoVerify = m => mockVerified = m.Equals(member) };
            // Act
            sut.Verify(member);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyMemberInfoCorrectlyInvokesPropertyVerify(Type type)
        {
            // Arrange
            var member = type.GetProperties().Cast<MemberInfo>().First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = p => mockVerified = p.Equals(member) };
            // Act
            sut.Verify(member);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(TypeWithFields))]
        public void VerifyMemberInfoCorrectlyInvokesFieldInfoVerify(Type type)
        {
            // Arrange
            var member = type.GetFields().Cast<MemberInfo>().First();
            var mockVerified = false;
            var sut = new DelegatingIdiomaticAssertion { OnFieldInfoVerify = f => mockVerified = f.Equals(member) };
            // Act
            sut.Verify(member);
            // Assert
            Assert.True(mockVerified, "Mock verified.");
        }

        [Fact]
        public void VerifyNullConstructorInfoArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((ConstructorInfo[])null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyConstructorInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var ctors = type.GetConstructors();
            var observedConstructors = new List<ConstructorInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = observedConstructors.Add };
            // Act
            sut.Verify(ctors);
            // Assert
            Assert.True(ctors.SequenceEqual(observedConstructors));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyConstructorInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var ctors = type.GetConstructors().AsEnumerable();
            var observedConstructors = new List<ConstructorInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = observedConstructors.Add };
            // Act
            sut.Verify(ctors);
            // Assert
            Assert.True(ctors.SequenceEqual(observedConstructors));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyConstructorInfoDoesNotThrow(Type type)
        {
            // Arrange
            var ctor = type.GetConstructors().First();
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(ctor)));
        }

        [Fact]
        public void VerifyNullFieldInfoArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((FieldInfo[])null));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(TypeWithFields))]
        public void VerifyFieldInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var fields = type.GetFields().ToArray();
            var observedFields = new List<FieldInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnFieldInfoVerify = observedFields.Add };
            // Act
            sut.Verify(fields);
            // Assert
            Assert.True(fields.SequenceEqual(observedFields));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(TypeWithFields))]
        public void VerifyFieldInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var fields = type.GetFields().Select(f => f);
            var observedFields = new List<FieldInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnFieldInfoVerify = observedFields.Add };
            // Act
            sut.Verify(fields);
            // Assert
            Assert.True(fields.SequenceEqual(observedFields));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(TypeWithFields))]
        public void VerifyFieldInfoDoesNotThrow(Type type)
        {
            // Arrange
            var field = type.GetFields().First();
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(field)));
        }

        [Fact]
        public void VerifyNullMethodInfoArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo[])null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMethodInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var methods = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors())).ToArray();
            var observedMethods = new List<MethodInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfoVerify = observedMethods.Add };
            // Act
            sut.Verify(methods);
            // Assert
            Assert.True(methods.SequenceEqual(observedMethods));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMethodInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var methods = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
            var observedMethods = new List<MethodInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnMethodInfoVerify = observedMethods.Add };
            // Act
            sut.Verify(methods);
            // Assert
            Assert.True(methods.SequenceEqual(observedMethods));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        public void VerifyMethodInfoDoesNotThrow(Type type)
        {
            // Arrange
            var method = type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors())).First();
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(method)));
        }

        [Fact]
        public void VerifyNullPropertyInfoArrayThrows()
        {
            // Arrange
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((PropertyInfo[])null));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyPropertyInfoArrayCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var properties = type.GetProperties();
            var observedProperties = new List<PropertyInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = observedProperties.Add };
            // Act
            sut.Verify(properties);
            // Assert
            Assert.True(properties.SequenceEqual(observedProperties));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyPropertyInfosCorrectlyInvokesNextVerify(Type type)
        {
            // Arrange
            var properties = type.GetProperties().AsEnumerable();
            var observedProperties = new List<PropertyInfo>();
            var sut = new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = observedProperties.Add };
            // Act
            sut.Verify(properties);
            // Assert
            Assert.True(properties.SequenceEqual(observedProperties));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void VerifyPropertyInfoDoesNotThrow(Type type)
        {
            // Arrange
            var property = type.GetProperties().First();
            var sut = new DelegatingIdiomaticAssertion();
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(property)));
        }

        public class TypeWithFields
        {
            public string StrField1;
            public int IntField;
            public bool BoolField;
            public List<string> ListField;
        }
    }
}
