using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ObjectBuilderTest
    {
        [Fact]
        public void CreateAnonymousWillSetInt32Property()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<int> result = sut.Build<PropertyHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillSetInt32Field()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            FieldHolder<int> result = sut.Build<FieldHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotAttemptToSetReadOnlyProperty()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            ReadOnlyPropertyHolder<int> result = sut.Build<ReadOnlyPropertyHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotAttemptToSetReadOnlyField()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            ReadOnlyFieldHolder<int> result = sut.Build<ReadOnlyFieldHolder<int>>().CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        public void WithWillSetPropertyOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<string> result = sut.Build<PropertyHolder<string>>().With(ph => ph.Property, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Property);
            // Teardown
        }

        [Fact]
        public void WithWillSetFieldOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var fixture = new Fixture();
            // Exercise system
            FieldHolder<string> result = fixture.Build<FieldHolder<string>>().With(fh => fh.Field, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Field);
            // Teardown
        }

        [Fact]
        public void AnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<long, long>>().With(ph => ph.Property1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property1);
            // Teardown
        }

        [Fact]
        public void AnonymousWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<int, decimal>>().With(fh => fh.Field1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field1);
            // Teardown
        }

        [Fact]
        public void WithoutWillIgnorePropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        public void WithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoProperties()
        {
            // Fixture setup
            var sut = new Fixture() { OmitAutoProperties = true };
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().WithAutoProperties().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        public void WithoutWillIgnoreFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Field1);
            // Teardown
        }

        [Fact]
        public void WithoutWillNotIgnoreOtherPropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoublePropertyHolder<string, string>>().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void WithoutWillNotIgnoreOtherFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            var result = sut.Build<DoubleFieldHolder<string, string>>().Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Field2);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesWillNotAutoPopulateProperty()
        {
            // Fixture setup
            var sut = new Fixture();
            // Exercise system
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesWillAutoPopulateProperty()
        {
            // Fixture setup
            var sut = new Fixture { OmitAutoProperties = true };
            // Exercise system
            PropertyHolder<object> result = sut.Build<PropertyHolder<object>>().WithAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        public void DoWillPerformOperationOnCreatedObject()
        {
            // Fixture setup
            var sut = new Fixture();
            var expectedObject = new object();
            // Exercise system
            var result = sut.Build<CollectionHolder<object>>().Do(x => x.Collection.Add(expectedObject)).CreateAnonymous().Collection.First();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuilderSequenceWillBePreserved()
        {
            // Fixture setup
            var sut = new Fixture();
            int expectedValue = 3;
            // Exercise system
            var result = sut.Build<PropertyHolder<int>>()
                .With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillInvokeResolve()
        {
            // Fixture setup
            bool resolveWasInvoked = false;

            var sut = new Fixture();
            sut.Resolver = t =>
                {
                    resolveWasInvoked = true;
                    return new ConcreteType();
                };
            // Exercise system
            sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous();
            // Verify outcome
            Assert.True(resolveWasInvoked, "Resolve");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResolveCallbackWithCorrectType()
        {
            // Fixture setup
            var sut = new Fixture();
            sut.Resolver = t =>
                {
                    Assert.Equal<Type>(typeof(AbstractType), t);
                    return new ConcreteType();
                };
            // Exercise system
            sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous();
            // Verify outcome (done by callback)
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResolveCallback()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();

            var sut = new Fixture();
            sut.Resolver = t => expectedValue;
            // Exercise system
            var result = sut.Build<PropertyHolder<AbstractType>>().CreateAnonymous().Property;
            // Verify outcome
            Assert.Equal<AbstractType>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropetiesWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.OmitAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        [Fact]
        public void WithAutoPropetiesWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.WithAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        public void AnonymousWithWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>().OmitAutoProperties();
            // Exercise system
            sut.With(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        public void WithWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var unexpectedProperty = "Anonymous value";
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.With(s => s.Property, unexpectedProperty);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotEqual(unexpectedProperty, instance.Property);
            // Teardown
        }

        [Fact]
        public void WithoutWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.Build<PropertyHolder<string>>();
            // Exercise system
            sut.Without(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
            // Teardown
        }
    }
}
