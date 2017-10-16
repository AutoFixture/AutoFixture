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
            // Fixture setup
            // Exercise system
            var sut = new CompositeIdiomaticAssertion();
            // Verify outcome
            Assert.IsAssignableFrom<IIdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructedWithArrayIdiomaticAssertionsIsCorrect()
        {
            // Fixture setup
            var assertions = new[]
            {
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion()
            };

            var sut = new CompositeIdiomaticAssertion(assertions);
            // Exercise system
            IEnumerable<IIdiomaticAssertion> result = sut.Assertions;
            // Verify outcome
            Assert.True(assertions.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ConstructedWithEnumerableIdiomaticAssertionsIsCorrect()
        {
            // Fixture setup
            IEnumerable<IIdiomaticAssertion> assertions = new[]
            {
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion()
            }.AsEnumerable();

            var sut = new CompositeIdiomaticAssertion(assertions);
            // Exercise system
            var result = sut.Assertions;
            // Verify outcome
            Assert.True(assertions.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void VerifyConstructorInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedConstructors = new List<ConstructorInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnConstructorInfoVerify = observedConstructors.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithConstructor = typeof (UnguardedConstructorHost<object>);
            ConstructorInfo ctor = typeWithConstructor.GetConstructors().First();
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome
            Assert.Equal(expectations.Length, observedConstructors.Count(ctor.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyMethodInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedMethods = new List<MethodInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnMethodInfoVerify = observedMethods.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithMethod = typeof (TypeWithConcreteParameterMethod);
            MethodInfo method = typeWithMethod.GetMethods().First();
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            Assert.Equal(expectations.Length, observedMethods.Count(method.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyPropertyInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedProperties = new List<PropertyInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnPropertyInfoVerify = observedProperties.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithMethod = typeof (PropertyHolder<object>);
            PropertyInfo property = typeWithMethod.GetProperties().First();
            // Exercise system
            sut.Verify(property);
            // Verify outcome
            Assert.Equal(expectations.Length, observedProperties.Count(property.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyFieldInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedFields = new List<FieldInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnFieldInfoVerify = observedFields.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithField = typeof (FieldHolder<object>);
            FieldInfo field = typeWithField.GetFields().First();
            // Exercise system
            sut.Verify(field);
            // Verify outcome
            Assert.Equal(expectations.Length, observedFields.Count(field.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyAssemblyVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedAssemblies = new List<Assembly>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnAssemblyVerify = observedAssemblies.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Assembly assembly = typeof (AbstractType).Assembly;
            // Exercise system
            sut.Verify(assembly);
            // Verify outcome
            Assert.Equal(expectations.Length, observedAssemblies.Count(assembly.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyTypeVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedTypes = new List<Type>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion {OnTypeVerify = observedTypes.Add}, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type type = typeof (AbstractType);
            // Exercise system
            sut.Verify(type);
            // Verify outcome
            Assert.Equal(expectations.Length, observedTypes.Count(type.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyAssemblyArrayVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(assemblies);
            // Verify outcome
            Assert.Equal(expectations.Length, observedAssemblies.Count(assemblies.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerableAssemblyVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(assemblies);
            // Verify outcome
            Assert.Equal(expectations.Length, observedAssemblies.Count(assemblies.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyTypeArrayVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(types);
            // Verify outcome
            Assert.Equal(expectations.Length, observedTypeArrays.Count(types.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerableTypesVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(assemblies);
            // Verify outcome
            Assert.Equal(expectations.Length, observedTypes.Count(assemblies.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyMemberInfoArrayVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(members);
            // Verify outcome
            Assert.Equal(expectations.Length, observedMemberInfoArrays.Count(members.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerableMemberInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(members);
            // Verify outcome
            Assert.Equal(expectations.Length, observedMemberInfos.Count(members.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerableConstructorInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(ctors);
            // Verify outcome
            Assert.Equal(expectations.Length, observedConstructors.Count(ctors.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerableMethodInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(methods);
            // Verify outcome
            Assert.Equal(expectations.Length, observedMethods.Count(methods.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyPropertyInfoArrayVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(properties);
            // Verify outcome
            Assert.Equal(expectations.Length, observedPropertyArrays.Count(properties.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerablePropertyInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(properties);
            // Verify outcome
            Assert.Equal(expectations.Length, observedProperties.Count(properties.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyFieldInfoArrayVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(fields);
            // Verify outcome
            Assert.Equal(expectations.Length, observedFieldArrays.Count(fields.Equals));
            // Teardown
        }

        [Fact]
        public void VerifyEnumerableFieldInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
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

            // Exercise system
            sut.Verify(fields);
            // Verify outcome
            Assert.Equal(expectations.Length, observedFields.Count(fields.Equals));
            // Teardown
        }
    }
}
