using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Idioms;
using Xunit;
using System.Reflection;
using System;

namespace Ploeh.AutoFixture.IdiomsUnitTest
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
            var assertions = new[]
            {
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion(),
                new DelegatingIdiomaticAssertion()
            };

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
                new DelegatingIdiomaticAssertion { OnConstructorInfoVerify = observedConstructors.Add }, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithConstructor = typeof(Ploeh.TestTypeFoundation.UnguardedConstructorHost<object>);
            ConstructorInfo ctor = typeWithConstructor.GetConstructors().First(); 
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome
            Assert.Equal(expectations.Length, observedConstructors.Count(c => ctor.Equals(c)));
            // Teardown
        }

        [Fact]
        public void VerifyMethodInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedMethods = new List<MethodInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion { OnMethodInfoVerify = observedMethods.Add }, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithMethod = typeof(Ploeh.TestTypeFoundation.TypeWithConcreteParameterMethod);
            MethodInfo method = typeWithMethod.GetMethods().First();
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            Assert.Equal(expectations.Length, observedMethods.Count(m => method.Equals(m)));
            // Teardown
        }

        [Fact]
        public void VerifyPropertyInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedProperties = new List<PropertyInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion { OnPropertyInfoVerify = observedProperties.Add }, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithMethod = typeof(Ploeh.TestTypeFoundation.PropertyHolder<object>);
            PropertyInfo property = typeWithMethod.GetProperties().First();
            // Exercise system
            sut.Verify(property);
            // Verify outcome
            Assert.Equal(expectations.Length, observedProperties.Count(p => property.Equals(p)));
            // Teardown
        }

        [Fact]
        public void VerifyFieldInfoVerifiesAllIdiomaticAssertions()
        {
            // Fixture setup
            var observedFields = new List<FieldInfo>();
            var expectations = Enumerable.Repeat(
                new DelegatingIdiomaticAssertion { OnFieldInfoVerify = observedFields.Add }, 3).ToArray();

            var sut = new CompositeIdiomaticAssertion(expectations);
            Type typeWithField = typeof(Ploeh.TestTypeFoundation.FieldHolder<object>);
            FieldInfo field = typeWithField.GetFields().First();
            // Exercise system
            sut.Verify(field);
            // Verify outcome
            Assert.Equal(expectations.Length, observedFields.Count(f => field.Equals(f)));
            // Teardown
        }
    }
}
