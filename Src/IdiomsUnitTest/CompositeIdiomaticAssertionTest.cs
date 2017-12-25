using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Idioms;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class CompositeIdiomaticAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            // Act
            var sut = new CompositeIdiomaticAssertion();
            // Assert
            Assert.IsAssignableFrom<IIdiomaticAssertion>(sut);
        }

        [Fact]
        public void ConstructedWithArrayIdiomaticAssertionsIsCorrect()
        {
            // Arrange
            var assertions = new[]
            {
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion()
            };

            var sut = new CompositeIdiomaticAssertion(assertions);
            // Act
            IEnumerable<IIdiomaticAssertion> result = sut.Assertions;
            // Assert
            Assert.True(assertions.SequenceEqual(result));
        }

        [Fact]
        public void ConstructedWithEnumerableIdiomaticAssertionsIsCorrect()
        {
            // Arrange
            IEnumerable<IIdiomaticAssertion> assertions = new[]
            {
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion()
            }.AsEnumerable();

            var sut = new CompositeIdiomaticAssertion(assertions);
            // Act
            var result = sut.Assertions;
            // Assert
            Assert.True(assertions.SequenceEqual(result));
        }

        [Fact]
        public void VerifyConstructorInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedConstructors = new List<ConstructorInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnConstructorInfoVerify = observedConstructors.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithConstructor = typeof (UnguardedConstructorHost<object>);
            ConstructorInfo ctor = typeWithConstructor.GetConstructors().First();
            // Act
            sut.Verify(ctor);
            // Assert
            Assert.Equal(expectations.Length, observedConstructors.Count(ctor.Equals));
        }

        [Fact]
        public void VerifyMethodInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedMethods = new List<MethodInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnMethodInfoVerify = observedMethods.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithMethod = typeof (TypeWithConcreteParameterMethod);
            MethodInfo method = typeWithMethod.GetMethods().First();
            // Act
            sut.Verify(method);
            // Assert
            Assert.Equal(expectations.Length, observedMethods.Count(method.Equals));
        }

        [Fact]
        public void VerifyPropertyInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedProperties = new List<PropertyInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnPropertyInfoVerify = observedProperties.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithMethod = typeof (PropertyHolder<object>);
            PropertyInfo property = typeWithMethod.GetProperties().First();
            // Act
            sut.Verify(property);
            // Assert
            Assert.Equal(expectations.Length, observedProperties.Count(property.Equals));
        }

        [Fact]
        public void VerifyFieldInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedFields = new List<FieldInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnFieldInfoVerify = observedFields.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithField = typeof (FieldHolder<object>);
            FieldInfo field = typeWithField.GetFields().First();
            // Act
            sut.Verify(field);
            // Assert
            Assert.Equal(expectations.Length, observedFields.Count(field.Equals));
        }

        [Fact]
        public void VerifyAssemblyVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedAssemblies = new List<Assembly>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnAssemblyVerify = observedAssemblies.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Assembly assembly = typeof (AbstractType).Assembly;
            // Act
            sut.Verify(assembly);
            // Assert
            Assert.Equal(expectations.Length, observedAssemblies.Count(assembly.Equals));
        }

        [Fact]
        public void VerifyTypeVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedTypes = new List<Type>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnTypeVerify = observedTypes.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type type = typeof (AbstractType);
            // Act
            sut.Verify(type);
            // Assert
            Assert.Equal(expectations.Length, observedTypes.Count(type.Equals));
        }

        [Fact]
        public void VerifyAssemblyArrayVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedAssemblies = new List<Assembly[]>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnAssemblyArrayVerify = observedAssemblies.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var assemblies = new Assembly[]
            {
                typeof (AbstractType).Assembly,
                typeof (System.String).Assembly,
            };

            // Act
            sut.Verify(assemblies);
            // Assert
            Assert.Equal(expectations.Length, observedAssemblies.Count(assemblies.Equals));
        }

        [Fact]
        public void VerifyEnumerableAssemblyVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedAssemblies = new List<IEnumerable<Assembly>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnAssembliesVerify = observedAssemblies.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var assemblies = new[]
            {
                typeof (AbstractType).Assembly,
                typeof (System.String).Assembly,
            }
                .AsEnumerable();

            // Act
            sut.Verify(assemblies);
            // Assert
            Assert.Equal(expectations.Length, observedAssemblies.Count(assemblies.Equals));
        }

        [Fact]
        public void VerifyTypeArrayVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedTypeArrays = new List<Type[]>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnTypeArrayVerify = observedTypeArrays.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var types = new[]
            {
                typeof (AbstractType),
                typeof (System.String),
            };

            // Act
            sut.Verify(types);
            // Assert
            Assert.Equal(expectations.Length, observedTypeArrays.Count(types.Equals));
        }

        [Fact]
        public void VerifyEnumerableTypesVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedTypes = new List<IEnumerable<Type>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnTypesVerify = observedTypes.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var assemblies = new[]
            {
                typeof (AbstractType),
                typeof (System.String),
            }
                .AsEnumerable();

            // Act
            sut.Verify(assemblies);
            // Assert
            Assert.Equal(expectations.Length, observedTypes.Count(assemblies.Equals));
        }

        [Fact]
        public void VerifyMemberInfoArrayVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedMemberInfoArrays = new List<MemberInfo[]>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnMemberInfoArrayVerify = observedMemberInfoArrays.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var members = new[]
            {
                typeof (AbstractType).GetMembers().First(),
                typeof (System.String).GetMembers().First(),
            };

            // Act
            sut.Verify(members);
            // Assert
            Assert.Equal(expectations.Length, observedMemberInfoArrays.Count(members.Equals));
        }

        [Fact]
        public void VerifyEnumerableMemberInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedMemberInfos = new List<IEnumerable<MemberInfo>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnMemberInfosVerify = observedMemberInfos.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            IEnumerable<MemberInfo> members = new[]
            {
                typeof (AbstractType).GetMembers().First(),
                typeof (System.String).GetMembers().First(),
            }
                .AsEnumerable();

            // Act
            sut.Verify(members);
            // Assert
            Assert.Equal(expectations.Length, observedMemberInfos.Count(members.Equals));
        }

        [Fact]
        public void VerifyEnumerableConstructorInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedConstructors = new List<IEnumerable<ConstructorInfo>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnConstructorInfosVerify = observedConstructors.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            IEnumerable<ConstructorInfo> ctors = new[]
            {
                typeof (ConcreteType).GetConstructors().First(),
                typeof (System.String).GetConstructors().First(),
            }
                .AsEnumerable();

            // Act
            sut.Verify(ctors);
            // Assert
            Assert.Equal(expectations.Length, observedConstructors.Count(ctors.Equals));
        }

        [Fact]
        public void VerifyEnumerableMethodInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedMethods = new List<IEnumerable<MethodInfo>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnMethodInfosVerify = observedMethods.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            IEnumerable<MethodInfo> methods = new[]
            {
                typeof (AbstractType).GetMethods().First(),
                typeof (System.String).GetMethods().First(),
            }
                .AsEnumerable();

            // Act
            sut.Verify(methods);
            // Assert
            Assert.Equal(expectations.Length, observedMethods.Count(methods.Equals));
        }

        [Fact]
        public void VerifyPropertyInfoArrayVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedPropertyArrays = new List<PropertyInfo[]>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnPropertyInfoArrayVerify = observedPropertyArrays.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var properties = new[]
            {
                typeof (AbstractType).GetProperties().First(),
                typeof (System.String).GetProperties().First(),
            };

            // Act
            sut.Verify(properties);
            // Assert
            Assert.Equal(expectations.Length, observedPropertyArrays.Count(properties.Equals));
        }

        [Fact]
        public void VerifyEnumerablePropertyInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedProperties = new List<IEnumerable<PropertyInfo>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnPropertyInfosVerify = observedProperties.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            IEnumerable<PropertyInfo> properties = new[]
            {
                typeof (AbstractType).GetProperties().First(),
                typeof (System.String).GetProperties().First(),
            }
                .AsEnumerable();

            // Act
            sut.Verify(properties);
            // Assert
            Assert.Equal(expectations.Length, observedProperties.Count(properties.Equals));
        }

        [Fact]
        public void VerifyFieldInfoArrayVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedFieldArrays = new List<FieldInfo[]>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnFieldInfoArrayVerify = observedFieldArrays.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            var fields = new[]
            {
                typeof (FieldHolder<object>).GetFields().First(),
                typeof (System.String).GetFields().First(),
            };

            // Act
            sut.Verify(fields);
            // Assert
            Assert.Equal(expectations.Length, observedFieldArrays.Count(fields.Equals));
        }

        [Fact]
        public void VerifyEnumerableFieldInfoVerifiesAllIdiomaticAssertions()
        {
            // Arrange
            var observedFields = new List<IEnumerable<FieldInfo>>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnFieldInfosVerify = observedFields.Add}, 3)
                .ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            IEnumerable<FieldInfo> fields = new[]
            {
                typeof (FieldHolder<object>).GetFields().First(),
                typeof (System.String).GetFields().First(),
            }
                .AsEnumerable();

            // Act
            sut.Verify(fields);
            // Assert
            Assert.Equal(expectations.Length, observedFields.Count(fields.Equals));
        }
    }
}
