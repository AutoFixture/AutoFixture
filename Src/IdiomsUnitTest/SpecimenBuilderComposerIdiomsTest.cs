using System;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Xunit;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class SpecimenBuilderComposerIdiomsTest
    {
        [Fact]
        public void ForPropertyWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Func<PropertyHolder<object>, object>> dummyExpression = ph => ph.Property;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForProperty(null, dummyExpression));
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
                .ForProperty((GuardedPropertyHolder<object> iph) => iph.Property)
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
                SpecimenBuilderComposerIdioms.ForProperty(null, dummyProperty));
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
            var ctx = Assert.IsAssignableFrom<PropertyContext>(result);
            Assert.Equal(fixture, ctx.Composer);
            Assert.Equal(anonymousProperty, ctx.PropertyInfo);
            // Teardown
        }

        [Fact]
        public void ForParameterlessMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers>> dummyExpression = a => a.DoSomething();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForMethod(null, dummyExpression));
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
            IMethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(fixture, ctx.Composer);
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
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(expectedMethodInfo, ctx.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForSingleParameterMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers, object>> dummyExpression = (a, x) => a.DoSomething(x);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForMethod(null, dummyExpression));
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
            IMethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(fixture, ctx.Composer);
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
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(expectedMethodInfo, ctx.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForDoubleParameterMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers, object, object>> dummyExpression = (a, x, y) => a.DoSomething(x, y);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForMethod(null, dummyExpression));
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
            IMethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(fixture, ctx.Composer);
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
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(expectedMethodInfo, ctx.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForTripleParameterMethodWithNullFixtureThrows()
        {
            // Fixture setup
            Expression<Action<TypeWithOverloadedMembers, object, object, object>> dummyExpression = (a, x, y, z) => a.DoSomething(x, y, z);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForMethod(null, dummyExpression));
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
            IMethodContext result = fixture.ForMethod(dummyExpression);
            // Verify outcome
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(fixture, ctx.Composer);
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
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(expectedMethodInfo, ctx.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForMethodWithNullFixtureAndDummyMethodInfoThrows()
        {
            // Fixture setup
            var dummyMethod = typeof(object).GetMethods().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForMethod(null, dummyMethod));
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
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(fixture, ctx.Composer);
            Assert.Equal(method, ctx.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForConstructorWithNullFixtureAndDummyConstructorInfoThrows()
        {
            // Fixture setup
            var dummyConstructor = typeof(object).GetConstructors().First();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForConstructor(null, dummyConstructor));
            // Teardown
        }

        [Fact]
        public void ForConstructorWithNullConstructorInfoThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForConstructor((ConstructorInfo)null));
            // Teardown
        }

        [Fact]
        public void ForConstructorWithConstructorInfoReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            var constructor = typeof(object).GetConstructors().First();
            // Exercise system
            var result = fixture.ForConstructor(constructor);
            // Verify outcome
            var ctx = Assert.IsAssignableFrom<MethodContext>(result);
            Assert.Equal(fixture, ctx.Composer);
            Assert.Equal(constructor, ctx.MethodBase);
            // Teardown
        }

        [Fact]
        public void ForAllMethodsOfGenericWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllMethodsOf<object>(null));
            // Teardown
        }

        [Fact]
        public void ForAllMethodsOfGenericReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllMethodsOf<PropertyHolder<object>>();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeMethodContext>(result);
            Assert.True((from ctx in composite.MethodContexts.OfType<MethodContext>()
                         let mb = ctx.MethodBase
                         orderby mb.Name
                         select mb).SequenceEqual(from m in typeof(PropertyHolder<object>).GetMethods().Except(typeof(PropertyHolder<object>).GetProperties().SelectMany(p => p.GetAccessors()))
                                                  where m.Name != "Equals"
                                                  orderby m.Name
                                                  select m as MethodBase));
            // Teardown
        }

        [Fact]
        public void ForAllMethodsOfTypeInstanceWithNullComposerThrows()
        {
            // Fixture setup
            var dummyType = typeof(object);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllMethodsOf(null, dummyType));
            // Teardown
        }

        [Fact]
        public void ForAllMethodsOfTypeInstanceWithNullTypeThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForAllMethodsOf(null));
            // Teardown
        }

        [Fact]
        public void ForAllMethodsOfTypeInstanceReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllMethodsOf(typeof(PropertyHolder<object>));
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeMethodContext>(result);
            Assert.True((from ctx in composite.MethodContexts.OfType<MethodContext>()
                         let mb = ctx.MethodBase
                         orderby mb.Name
                         select mb).SequenceEqual(from m in typeof(PropertyHolder<object>).GetMethods().Except(typeof(PropertyHolder<object>).GetProperties().SelectMany(p => p.GetAccessors()))
                                                  where m.Name != "Equals"
                                                  orderby m.Name
                                                  select m as MethodBase));
            // Teardown
        }

        [Fact]
        public void ForAllPropertiesOfGenericWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllPropertiesOf<PropertyHolder<string>>(null));
            // Teardown
        }

        [Fact]
        public void ForAllPropertiesOfGenericReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllPropertiesOf<PropertyHolder<string>>();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePropertyContext>(result);
            Assert.True((from ctx in composite.PropertyContexts.OfType<PropertyContext>()
                         let pi = ctx.PropertyInfo
                         orderby pi.Name
                         select pi).SequenceEqual(from p in typeof(PropertyHolder<string>).GetProperties()
                                                  orderby p.Name
                                                  select p));
            // Teardown
        }

        [Fact]
        public void ForAllPropertiesOfTypeInstanceWithNullComposerThrows()
        {
            // Fixture setup
            var dummyType = typeof(PropertyHolder<Guid>);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllPropertiesOf(null, dummyType));
            // Teardown
        }

        [Fact]
        public void ForAllPropertiesOfTypeInstanceWithNullTypeThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForAllPropertiesOf(null));
            // Teardown
        }

        [Fact]
        public void ForAllPropertiesOfTypeInstanceReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllPropertiesOf(typeof(PropertyHolder<string>));
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositePropertyContext>(result);
            Assert.True((from ctx in composite.PropertyContexts.OfType<PropertyContext>()
                         let pi = ctx.PropertyInfo
                         orderby pi.Name
                         select pi).SequenceEqual(from p in typeof(PropertyHolder<string>).GetProperties()
                                                  orderby p.Name
                                                  select p));
            // Teardown
        }

        [Fact]
        public void ForAllConstructorsOfGenericWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllConstructorsOf<GuardedConstructorHost<Version>>(null));
            // Teardown
        }

        [Fact]
        public void ForAllConstructorsOfGenericReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllConstructorsOf<GuardedConstructorHost<Version>>();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeMethodContext>(result);
            Assert.True((from ctx in composite.MethodContexts.OfType<MethodContext>()
                         let m = ctx.MethodBase
                         orderby m.Name
                         select m).SequenceEqual(from c in typeof(GuardedConstructorHost<Version>).GetConstructors()
                                                  orderby c.Name
                                                  select c as MethodBase));
            // Teardown
        }

        [Fact]
        public void ForAllConstructorsOfTypeInstanceWithNullComposerThrows()
        {
            // Fixture setup
            var dummyType = typeof(GuardedConstructorHost<Version>);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllConstructorsOf(null, dummyType));
            // Teardown
        }

        [Fact]
        public void ForAllConstructorsOfTypeInstanceWithNullTypeThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForAllConstructorsOf(null));
            // Teardown
        }

        [Fact]
        public void ForAllConstructorsOfTypeInstanceReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllConstructorsOf(typeof(GuardedConstructorHost<Version>));
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeMethodContext>(result);
            Assert.True((from ctx in composite.MethodContexts.OfType<MethodContext>()
                         let m = ctx.MethodBase
                         orderby m.Name
                         select m).SequenceEqual(from c in typeof(GuardedConstructorHost<Version>).GetConstructors()
                                                 orderby c.Name
                                                 select c as MethodBase));
            // Teardown
        }

        [Fact]
        public void ForAllMembersOfGenericWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllMembersOf<GuardedConstructorHost<Version>>(null));
            // Teardown
        }

        [Fact]
        public void ForAllMembersOfGenericReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllMembersOf<GuardedConstructorHost<Version>>();
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeMemberContext>(result);

            var constructorComposite = Assert.IsAssignableFrom<CompositeMethodContext>(composite.MemberContexts.First());
            Assert.True((from ctx in constructorComposite.MethodContexts.OfType<MethodContext>()
                         let m = ctx.MethodBase
                         orderby m.Name
                         select m).SequenceEqual(from c in typeof(GuardedConstructorHost<Version>).GetConstructors()
                                                 orderby c.Name
                                                 select c as MethodBase));

            var methodComposite = Assert.IsAssignableFrom<CompositeMethodContext>(composite.MemberContexts.ElementAt(1));
            Assert.True((from ctx in methodComposite.MethodContexts.OfType<MethodContext>()
                         let mb = ctx.MethodBase
                         orderby mb.Name
                         select mb).SequenceEqual(from m in typeof(GuardedConstructorHost<Version>).GetMethods().Except(typeof(GuardedConstructorHost<Version>).GetProperties().SelectMany(p => p.GetAccessors()))
                                                  where m.Name != "Equals"
                                                  orderby m.Name
                                                  select m as MethodBase));

            var propertyComposite = Assert.IsAssignableFrom<CompositePropertyContext>(composite.MemberContexts.ElementAt(2));
            Assert.True((from ctx in propertyComposite.PropertyContexts.OfType<PropertyContext>()
                         let pi = ctx.PropertyInfo
                         orderby pi.Name
                         select pi).SequenceEqual(from p in typeof(GuardedConstructorHost<Version>).GetProperties()
                                                  orderby p.Name
                                                  select p));
            // Teardown
        }

        [Fact]
        public void ForAllMembersOfTypeInstanceWithNullComposerThrows()
        {
            // Fixture setup
            var dummyType = typeof(GuardedConstructorHost<Version>);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                SpecimenBuilderComposerIdioms.ForAllMembersOf(null, dummyType));
            // Teardown
        }

        [Fact]
        public void ForAllMembersOfTypeInstanceWithNullTypeThrows()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.ForAllMembersOf(null));
            // Teardown
        }

        [Fact]
        public void ForAllMembersOfTypeInstanceReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var result = fixture.ForAllMembersOf(typeof(GuardedConstructorHost<Version>));
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeMemberContext>(result);

            var constructorComposite = Assert.IsAssignableFrom<CompositeMethodContext>(composite.MemberContexts.First());
            Assert.True((from ctx in constructorComposite.MethodContexts.OfType<MethodContext>()
                         let m = ctx.MethodBase
                         orderby m.Name
                         select m).SequenceEqual(from c in typeof(GuardedConstructorHost<Version>).GetConstructors()
                                                 orderby c.Name
                                                 select c as MethodBase));

            var methodComposite = Assert.IsAssignableFrom<CompositeMethodContext>(composite.MemberContexts.ElementAt(1));
            Assert.True((from ctx in methodComposite.MethodContexts.OfType<MethodContext>()
                         let mb = ctx.MethodBase
                         orderby mb.Name
                         select mb).SequenceEqual(from m in typeof(GuardedConstructorHost<Version>).GetMethods().Except(typeof(GuardedConstructorHost<Version>).GetProperties().SelectMany(p => p.GetAccessors()))
                                                  where m.Name != "Equals"
                                                  orderby m.Name
                                                  select m as MethodBase));

            var propertyComposite = Assert.IsAssignableFrom<CompositePropertyContext>(composite.MemberContexts.ElementAt(2));
            Assert.True((from ctx in propertyComposite.PropertyContexts.OfType<PropertyContext>()
                         let pi = ctx.PropertyInfo
                         orderby pi.Name
                         select pi).SequenceEqual(from p in typeof(GuardedConstructorHost<Version>).GetProperties()
                                                  orderby p.Name
                                                  select p));
            // Teardown
        }
    }
}
