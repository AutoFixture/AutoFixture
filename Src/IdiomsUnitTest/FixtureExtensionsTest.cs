using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class FixtureExtensionsTest
    {
        [Fact]
        public void ForPropertyWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> dummyExpression = ph => ph.Property;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForProperty(null, dummyExpression));
            // Teardown
        }

        [Fact]
        public void ForPropertyWithNullExpressionWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentNullException), () =>
                new Fixture().ForProperty<object, object>(null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ForPropertyWithNullFuncWillThrow()
        {
            // Fixture setup
            // Exercise system
            Assert.Throws(typeof(ArgumentException), () =>
                new Fixture().ForProperty<object, object>(sut => (object)null));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void ForPropertyCanPickReadOnlyProperty()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().ForProperty((ReadOnlyPropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ForPropertyWithReadWritePropertyWillReturnInstance()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().ForProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ForPropertyWithReadWritePropertyWillReturnCorrectType()
        {
            // Fixture setup
            // Exercise system
            var result = new Fixture().ForProperty((PropertyHolder<object> sut) => sut.Property);
            // Verify outcome
            Assert.IsAssignableFrom<PropertyContext>(result);
            // Teardown
        }

        [Fact]
        public void ForPropertyCanVerifyWritable()
        {
            // Fixture setup
            // Exercise system
            new Fixture()
                .ForProperty((PropertyHolder<object> ph) => ph.Property)
                .VerifyWritable();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void ForPropertyVerifyBoundaries()
        {
            // Fixture setup
            // Exercise system
            new Fixture()
                .ForProperty((InvariantReferenceTypePropertyHolder<object> iph) => iph.Property)
                .VerifyBoundaries();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void ForPropertyWithNullFixtureAndDummyPropertyInfoThrows()
        {
            // Fixture setup
            var dummyProperty = typeof(string).GetProperties().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForProperty(null, dummyProperty));
            // Teardown
        }

        [Fact]
        public void ForPropertyWithNullPropertyInfoThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForProperty((PropertyInfo)null));
            // Teardown
        }

        [Fact]
        public void ForPropertyWithPropertyInfoReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var anonymousProperty = typeof(string).GetProperties().First();
            // Exercise system
            var result = fixture.ForProperty(anonymousProperty);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            Assert.Equal(anonymousProperty, result.PropertyInfo);
            // Teardown
        }

        [Fact]
        public void ForParameterlessMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers>> dummyExpression = a => a.DoSomething();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForMethod(null, dummyExpression));
            // Teardown
        }

        [Fact]
        public void ForParameterlessMethodWithNullExpressionThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForMethod<object>(null));
            // Teardown
        }

        [Fact]
        public void ForParameterlessMethodReturnsResultWithCorrectFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            Expression<Action<TypeWithOverloadedMembers>> dummyExpression = a => a.DoSomething();
            // Exercise system
            MethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            // Teardown
        }

        [Fact]
        public void ForParameterlessMethodReturnsResultWithCorrectMethodInfo()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedMethodInfo = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", Type.EmptyTypes);
            // Exercise system
            var result = fixture.ForMethod((TypeWithOverloadedMembers a) => a.DoSomething());
            // Verify outcome
            Assert.Equal(expectedMethodInfo, result.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForSingleParameterMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers, object>> dummyExpression = (a, x) => a.DoSomething(x);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForMethod(null, dummyExpression));
            // Teardown
        }

        [Fact]
        public void ForSingleParameterMethodWithNullExpressionThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForMethod<object, object>(null));
            // Teardown
        }

        [Fact]
        public void ForSingleParameterMethodReturnsResultWithCorrectFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            Expression<Action<TypeWithOverloadedMembers, object>> dummyExpression = (a, x) => a.DoSomething(x);
            // Exercise system
            MethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            // Teardown
        }

        [Fact]
        public void ForSingleParameterMethodReturnsResultWithCorrectMethodInfo()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedMethodInfo = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            // Exercise system
            var result = fixture.ForMethod((TypeWithOverloadedMembers a, object x) => a.DoSomething(x));
            // Verify outcome
            Assert.Equal(expectedMethodInfo, result.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForDoubleParameterMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers, object, object>> dummyExpression = (a, x, y) => a.DoSomething(x, y);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForMethod(null, dummyExpression));
            // Teardown
        }

        [Fact]
        public void ForDoubleParameterMethodWithNullExpressionThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForMethod<object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void ForDoubleParameterMethodReturnsResultWithCorrectFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            Expression<Action<TypeWithOverloadedMembers, object, object>> dummyExpression = (a, x, y) => a.DoSomething(x, y);
            // Exercise system
            MethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            // Teardown
        }

        [Fact]
        public void ForDoubleParameterMethodReturnsResultWithCorrectMethodInfo()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedMethodInfo = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object), typeof(object) });
            // Exercise system
            var result = fixture.ForMethod((TypeWithOverloadedMembers a, object x, object y) => a.DoSomething(x, y));
            // Verify outcome
            Assert.Equal(expectedMethodInfo, result.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForTripleParameterMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers, object, object, object>> dummyExpression = (a, x, y, z) => a.DoSomething(x, y, z);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForMethod(null, dummyExpression));
            // Teardown
        }

        [Fact]
        public void ForTripleParameterMethodWithNullExpressionThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForMethod<object, object, object, object>(null));
            // Teardown
        }

        [Fact]
        public void ForTripleParameterMethodReturnsResultWithCorrectFixture()
        {
            // Fixture setup
            var fixture = new Fixture();
            Expression<Action<TypeWithOverloadedMembers, object, object, object>> dummyExpression = (a, x, y, z) => a.DoSomething(x, y, z);
            // Exercise system
            MethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            // Teardown
        }

        [Fact]
        public void ForTripleParameterMethodReturnsResultWithCorrectMethodInfo()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedMethodInfo = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            // Exercise system
            var result = fixture.ForMethod((TypeWithOverloadedMembers a, object x, object y, object z) => a.DoSomething(x, y, z));
            // Verify outcome
            Assert.Equal(expectedMethodInfo, result.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForMethodWithNullFixtureAndDummyMethodInfoThrows()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForMethod(null, dummyMethod));
            // Teardown
        }

        [Fact]
        public void ForMethodWithNullMethodInfoThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForMethod((MethodInfo)null));
            // Teardown
        }

        [Fact]
        public void ForMethodWithMethodInfoReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var method = typeof(object).GetMethods().First();
            // Exercise system
            var result = fixture.ForMethod(method);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            Assert.Equal(method, result.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForMethodWithNullFixtureAndDummyConstructorInfoThrows()
        {
            // Fixture setup
            var dummyConstructor = typeof(object).GetConstructors().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                FixtureExtensions.ForMethod(null, dummyConstructor));
            // Teardown
        }

        [Fact]
        public void ForMethodWithNullConstructorInfoThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForMethod((ConstructorInfo)null));
            // Teardown
        }

        [Fact]
        public void ForMethodWithConstructorInfoReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var constructor = typeof(object).GetConstructors().First();
            // Exercise system
            var result = fixture.ForMethod(constructor);
            // Verify outcome
            Assert.Equal(fixture, result.Composer);
            Assert.Equal(constructor, result.MethodBase);
            // Teardown
        }
    }
}
