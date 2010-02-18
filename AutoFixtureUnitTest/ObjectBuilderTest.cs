using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class ObjectBuilderTest
    {
        public ObjectBuilderTest()
        {
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullTypeMappingsWillThrow()
        {
            // Fixture setup
            IDictionary<Type, Func<object, object>> nullTypeMappings = null;
            // Exercise system
            new TestableObjectBuilder<object>(nullTypeMappings, 7, t => null, new object());
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillSetInt32Property()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            TestableObjectBuilder<PropertyHolder<int>> sut = ObjectBuilderTest.CreateSut(new PropertyHolder<int>());
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<int>(unexpectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillSetInt32Field()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            TestableObjectBuilder<FieldHolder<int>> sut = ObjectBuilderTest.CreateSut(new FieldHolder<int>());
            // Exercise system
            FieldHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<int>(unexpectedNumber, result.Field, "Field");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillNotAttemptToSetReadOnlyProperty()
        {
            // Fixture setup
            int expectedNumber = default(int);
            TestableObjectBuilder<ReadOnlyPropertyHolder<int>> sut = ObjectBuilderTest.CreateSut(new ReadOnlyPropertyHolder<int>());
            // Exercise system
            ReadOnlyPropertyHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<int>(expectedNumber, result.Property, "Property");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillNotAttemptToSetReadOnlyField()
        {
            // Fixture setup
            int expectedNumber = default(int);
            TestableObjectBuilder<ReadOnlyFieldHolder<int>> sut = ObjectBuilderTest.CreateSut(new ReadOnlyFieldHolder<int>());
            // Exercise system
            ReadOnlyFieldHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<int>(expectedNumber, result.Field, "Field");
            // Teardown
        }

        [TestMethod]
        public void WithWillSetPropertyOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            TestableObjectBuilder<PropertyHolder<string>> sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            PropertyHolder<string> result = sut.With(ph => ph.Property, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Property, "Property was correctly set");
            // Teardown
        }

        [TestMethod]
        public void WithWillSetFieldOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            TestableObjectBuilder<FieldHolder<string>> sut = ObjectBuilderTest.CreateSut(new FieldHolder<string>());
            // Exercise system
            FieldHolder<string> result = sut.With(fh => fh.Field, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>(expectedText, result.Field, "Field was correctly set");
            // Teardown
        }

        [TestMethod]
        public void AnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            var sut = ObjectBuilderTest.CreateSut(new DoublePropertyHolder<long, long>());
            // Exercise system
            var result = sut.With(ph => ph.Property1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<long>(unexpectedNumber, result.Property1, "Property should be assigned anonymous value");
            // Teardown
        }

        [TestMethod]
        public void AnonymousWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = ObjectBuilderTest.CreateSut(new DoubleFieldHolder<int, decimal>());
            // Exercise system
            var result = sut.With(fh => fh.Field1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.AreNotEqual<int>(unexpectedNumber, result.Field1, "Field should be assigned anonymous value");
            // Teardown
        }

        [TestMethod]
        public void WithoutWillIgnorePropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoublePropertyHolder<string, string>());
            // Exercise system
            var result = sut.Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.IsNull(result.Property1, "Property should be ignored");
            // Teardown
        }

        [TestMethod]
        public void WithoutWillIgnoreFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoubleFieldHolder<string, string>());
            // Exercise system
            var result = sut.Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.IsNull(result.Field1, "Field should be ignored");
            // Teardown
        }

        [TestMethod]
        public void WithoutWillNotIgnoreOtherPropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoublePropertyHolder<string, string>());
            // Exercise system
            var result = sut.Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(result.Property2, "Other property should not be ignored");
            // Teardown
        }

        [TestMethod]
        public void WithoutWillNotIgnoreOtherFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoubleFieldHolder<string, string>());
            // Exercise system
            var result = sut.Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.IsNotNull(result.Field2, "Other field should not be ignored");
            // Teardown
        }

        [TestMethod]
        public void OmitAutoPropertiesWillNotAutoPopulateProperty()
        {
            // Fixture setup
            TestableObjectBuilder<PropertyHolder<object>> sut = ObjectBuilderTest.CreateSut(new PropertyHolder<object>());
            // Exercise system
            PropertyHolder<object> result = sut.OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.IsNull(result.Property, "OmitProperties");
            // Teardown
        }

        [TestMethod]
        public void DoWillPerformOperationOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new CollectionHolder<object>());
            var expectedObject = new object();
            // Exercise system
            var result = sut.Do(x => x.Collection.Add(expectedObject)).CreateAnonymous().Collection.First();
            // Verify outcome
            Assert.AreEqual<object>(expectedObject, result, "Do");
            // Teardown
        }

        [TestMethod]
        public void BuilderSequenceWillBePreserved()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<int>());
            int expectedValue = 3;
            // Exercise system
            var result = sut.With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<int>(expectedValue, result.Property, "With/Do/With sequence");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousWillInvokeResolve()
        {
            // Fixture setup
            bool resolveWasInvoked = false;
            Func<Type, object> resolve = t =>
                {
                    resolveWasInvoked = true;
                    return new ConcreteType();
                };
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<AbstractType>(), resolve);
            // Exercise system
            sut.CreateAnonymous();
            // Verify outcome
            Assert.IsTrue(resolveWasInvoked, "Resolve");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResolveCallbackWithCorrectType()
        {
            // Fixture setup
            Func<Type, object> resolve = t =>
                {
                    Assert.AreEqual<Type>(typeof(AbstractType), t, "Resolve");
                    return new ConcreteType();
                };
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<AbstractType>(), resolve);
            // Exercise system
            sut.CreateAnonymous();
            // Verify outcome (done by callback)
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResolveCallback()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();
            Func<Type, object> resolve = t => expectedValue;
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<AbstractType>(), resolve);            
            // Exercise system
            var result = sut.CreateAnonymous().Property;
            // Verify outcome
            Assert.AreEqual<AbstractType>(expectedValue, result, "Resolve");
            // Teardown
        }

        [TestMethod]
        public void OmitAutoPropetiesWillNotMutateSut()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            sut.OmitAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.IsNotNull(instance.Property, "OmitAutoProperties");
            // Teardown
        }

        [TestMethod]
        public void AnonymousWithWillNotMutateSut()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>()).OmitAutoProperties();
            // Exercise system
            sut.With(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.IsNull(instance.Property, "With");
            // Teardown
        }

        [TestMethod]
        public void WithWillNotMutateSut()
        {
            // Fixture setup
            var unexpectedProperty = "Anonymous value";
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            sut.With(s => s.Property, unexpectedProperty);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.AreNotEqual(unexpectedProperty, instance.Property, "Wtih");
            // Teardown
        }

        [TestMethod]
        public void WithoutWillNotMutateSut()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            sut.Without(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.IsNotNull(instance.Property, "Without");
            // Teardown
        }

        private static TestableObjectBuilder<T> CreateSut<T>(T obj)
        {
            Fixture f = new Fixture();
            return ObjectBuilderTest.CreateSut(f, obj);
        }

        private static TestableObjectBuilder<T> CreateSut<T>(T obj, Func<Type, object> resolve)
        {
            var f = new Fixture();
#pragma warning disable 618
            return new TestableObjectBuilder<T>(f.TypeMappings, f.RepeatCount, resolve, obj);
#pragma warning restore 618
        }

        private static TestableObjectBuilder<T> CreateSut<T>(Fixture fixture, T obj)
        {
#pragma warning disable 618
            return new TestableObjectBuilder<T>(fixture.TypeMappings, fixture.RepeatCount, t => null, obj);
#pragma warning restore 618
        }

        private class TestableObjectBuilder<T> : ObjectBuilder<T>
        {
            private readonly IDictionary<Type, Func<object, object>> typeMappings;
            private readonly int repeatCount;
            private readonly Func<Type, object> resolve;

            internal TestableObjectBuilder(IDictionary<Type, Func<object, object>> typeMappings, int repeatCount, Func<Type, object> resolve, T obj)
                : base(typeMappings, repeatCount, resolve)
            {
                this.typeMappings = typeMappings;
                this.repeatCount = repeatCount;
                this.resolve = resolve;
                this.CreatedObject = obj;
            }

            internal T CreatedObject { get; private set; }

            protected override ObjectBuilder<T> CloneCore()
            {
                return new TestableObjectBuilder<T>(this.typeMappings, this.repeatCount, this.resolve, this.CreatedObject);
            }

            protected override T Create(T seed)
            {
                return this.CreatedObject;
            }
        }

    }
}
