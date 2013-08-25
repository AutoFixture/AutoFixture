using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    [TestFixture]
    public class CompositeDataAttributeTest
    {
        [Test]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeDataAttribute();
            // Verify outcome
            Assert.IsInstanceOf<DataAttribute>(sut);
            // Teardown
        }

        [Test]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
            // Teardown
        }

        [Test]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Test]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute((IEnumerable<DataAttribute>)null));
            // Teardown
        }

        [Test]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Test]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null, dummyTypes).ToList());
            // Teardown
        }

        [Test]
        public void GetDataWithNullTypesThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            Action a = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(a.Method, null).ToList());
            // Teardown
        }

        [Test]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var result = sut.GetData(a.Method, Type.EmptyTypes);
            Array.ForEach(result.ToArray(), Assert.IsEmpty);
            // Teardown
        }
    }
}