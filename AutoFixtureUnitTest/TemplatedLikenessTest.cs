using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    // Regression tests of obsolete API until it is removed completely
#pragma warning disable 618

    [TestClass]
    public class TemplatedLikenessTest
    {
        public TemplatedLikenessTest()
        {
        }

        [TestMethod]
        public void SutIsLikeness()
        {
            // Fixture setup
            // Exercise system
            var sut = new Likeness<object>(new object());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(Likeness));
            // Teardown
        }

        [TestMethod]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new Likeness<PropertyHolder<float>>(new PropertyHolder<float>());
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IEquatable<PropertyHolder<float>>));
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new Likeness<DateTime>(DateTime.Now);
            object nullObject = null;
            // Exercise system
            var result = sut.Equals(nullObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullValue()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<short>>(new PropertyHolder<short>());
            PropertyHolder<short> nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType>(new ConcreteType());
            object anonymousObject = new object();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsIdenticalStrongType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan>(value);
            var other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsIdenticalWeakType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan>(value);
            object other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "Ploeh";
            var sut = new Likeness<AbstractType>(value);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithoutPropertyEqualsInstanceEvenIfItDiffersOnThatProperty()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";
            var sut = new Likeness<ConcreteType>(value).Without(x => x.Property5);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithoutTwoPropertiesEqualsInstanceEvenThoughThosePropertyValuesDiffer()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";
            var sut = new Likeness<ConcreteType>(value).Without(x => x.Property5).Without(x => x.Property1);

            var other = new ConcreteType();
            other.Property1 = "Ndøh";
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Sqryt";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var sut = new Likeness<DoublePropertyHolder<string, string>>(value);

            var other = new DoublePropertyHolder<object, object>();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithValueThatDoesNotMatchTypeParameterWillReturnFalseEvenWhenComparedWithIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Ploeh = "ipsum" };
            var sut = new Likeness<DoublePropertyHolder<object, object>>(value);

            var other = new { Property1 = "Lorem", Ploeh = "ipsum" };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithAnonymousValueWillEqualSemanticallyIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Property2 = "ipsum" };
            var sut = new Likeness<DoublePropertyHolder<object, object>>(value);

            var other = new { Property1 = "Lorem", Property2 = "ipsum" };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }
    }

#pragma warning restore 618
}
