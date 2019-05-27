using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public partial class GuardClauseAssertionTest
    {
        private static Type[] UnguardedOpenGenericTypes => new[]
        {
            typeof(ClassContraint<>),
            typeof(CertainClassContraint<>),
            typeof(CertainClassAndInterfacesContraint<>),
            typeof(MultipleGenericArguments<,>),
            typeof(AbstractTypeAndInterfacesContraint<>),
            typeof(OpenGenericTestType<>).BaseType,
            typeof(ConstructedGenericTestType<>).BaseType,
            typeof(InternalProtectedConstructorTestConstraint<>),
            typeof(ModestConstructorTestConstraint<>),
            typeof(ConstructorMatchTestType<,>),
            typeof(MethodMatchTestType<,>),
            typeof(ByRefTestType<>)
        };

        public static TheoryData<MemberRef<ConstructorInfo>> ConstructorsOnUnguardedOpenGenericTypes =>
            MakeTheoryData(
                UnguardedOpenGenericTypes
                    .SelectMany(t => t.GetConstructors())
                    .Select(c => new MemberRef<ConstructorInfo>(c)));

        [Theory, MemberData(nameof(ConstructorsOnUnguardedOpenGenericTypes))]
        public void VerifyConstructorOnUnguardedGenericThrows(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var constructorInfo = ctorRef.Member;
            var sut = new GuardClauseAssertion(new Fixture { OmitAutoProperties = true });
            // Act
            // Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(constructorInfo));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        public static TheoryData<MemberRef<MethodInfo>> MethodsOnUnguardedOpenGenericTypes =>
            MakeTheoryData(
                UnguardedOpenGenericTypes
                    .SelectMany(t =>
                        t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    // Skip getters and setters
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
                    .Select(m => new MemberRef<MethodInfo>(m)));

        [Theory, MemberData(nameof(MethodsOnUnguardedOpenGenericTypes))]
        public void VerifyMethodOnUnguardedGenericThrows(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var methodInfo = methodRef.Member;
            var sut = new GuardClauseAssertion(new Fixture { OmitAutoProperties = true });
            // Act
            // Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(methodInfo));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        public static TheoryData<MemberRef<PropertyInfo>> PropertiesOnUnguardedOpenGenericTypes =>
            MakeTheoryData(
                UnguardedOpenGenericTypes
                    .SelectMany(t => t.GetProperties())
                    .Select(c => new MemberRef<PropertyInfo>(c)));

        [Theory, MemberData(nameof(PropertiesOnUnguardedOpenGenericTypes))]
        public void VerifyPropertyOnUnguardedGenericThrows(MemberRef<PropertyInfo> propertyRef)
        {
            // Arrange
            var propertyInfo = propertyRef.Member;
            var sut = new GuardClauseAssertion(new Fixture { OmitAutoProperties = true });
            // Act
            // Assert
            var e = Assert.Throws<GuardClauseException>(() => sut.Verify(propertyInfo));
            Assert.Contains("Are you missing a Guard Clause?", e.Message);
        }

        private static Type[] GuardedOpenGenericTypes => new[]
        {
            typeof(NoContraint<>),
            typeof(InterfacesContraint<>),
            typeof(StructureAndInterfacesContraint<>),
            typeof(ParameterizedConstructorTestConstraint<>),
            typeof(UnclosedGenericMethodTestType<>),
            typeof(NestedGenericParameterTestType<,>)
        };

        public static TheoryData<MemberRef<ConstructorInfo>> ConstructorsOnGuardedOpenGenericTypes =>
            MakeTheoryData(
                GuardedOpenGenericTypes
                    .SelectMany(t => t.GetConstructors())
                    .Select(c => new MemberRef<ConstructorInfo>(c)));

        [Theory, MemberData(nameof(ConstructorsOnGuardedOpenGenericTypes))]
        public void VerifyConstructorOnGuardedGenericDoesNotThrow(MemberRef<ConstructorInfo> ctorRef)
        {
            // Arrange
            var constructor = ctorRef.Member;
            var sut = new GuardClauseAssertion(new Fixture());
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(constructor)));
        }

        public static TheoryData<MemberRef<MethodInfo>> MethodsOnGuardedOpenGenericTypes =>
            MakeTheoryData(
                GuardedOpenGenericTypes
                    .SelectMany(t =>
                        t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    // Skip getters and setters
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
                    .Select(m => new MemberRef<MethodInfo>(m)));

        [Theory, MemberData(nameof(MethodsOnGuardedOpenGenericTypes))]
        public void VerifyMethodOnGuardedGenericDoesNotThrow(MemberRef<MethodInfo> methodRef)
        {
            // Arrange
            var methodInfo = methodRef.Member;
            var sut = new GuardClauseAssertion(new Fixture());
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(methodInfo)));
        }

        public static TheoryData<MemberRef<PropertyInfo>> PropertiesOnGuardedOpenGenericTypes =>
            MakeTheoryData(
                GuardedOpenGenericTypes
                    .SelectMany(t => t.GetProperties())
                    .Select(c => new MemberRef<PropertyInfo>(c)));

        [Theory, MemberData(nameof(PropertiesOnGuardedOpenGenericTypes))]
        public void VerifyPropertyOnGuardedGenericDoesNotThrow(MemberRef<PropertyInfo> propertyRef)
        {
            // Arrange
            var propertyInfo = propertyRef.Member;
            var sut = new GuardClauseAssertion(new Fixture());
            // Act
            // Assert
            Assert.Null(Record.Exception(() => sut.Verify(propertyInfo)));
        }

        [Fact]
        public void VerifyUnguardedConstructorOnGenericHavingNoAccessibleConstructorGenericArgumentThrows()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            var constructorInfo = typeof(NoAccessibleConstructorTestConstraint<>).GetConstructors().Single();
            // Act
            // Assert
            var e = Assert.Throws<ArgumentException>(() => sut.Verify(constructorInfo));
            Assert.Equal(
                "Cannot create a dummy type because the base type " +
                "'AutoFixture.IdiomsUnitTest.GuardClauseAssertionTest+NoAccessibleConstructorTestType' " +
                "does not have any accessible constructor.",
                e.Message);
        }

        [Fact]
        public void DynamicDummyTypeIfReturnMethodIsCalledReturnsAnonymousValue()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var objectValue = fixture.Freeze<object>();
            var intValue = fixture.Freeze<int>();

            bool mockVerification = false;
            var behaviorExpectation = new DelegatingBehaviorExpectation()
            {
                OnVerify = c =>
                {
                    var dynamicInstance = (IDynamicInstanceTestType)GetParameters(c).ElementAt(0);
                    Assert.Equal(objectValue, dynamicInstance.Property);
                    Assert.Equal(intValue, dynamicInstance.ReturnMethod(null, 123));
                    mockVerification = true;
                }
            };

            var sut = new GuardClauseAssertion(fixture, behaviorExpectation);
            var methodInfo = typeof(DynamicInstanceTestConstraint<>).GetMethod("Method");
            // Act
            sut.Verify(methodInfo);
            // Assert
            Assert.True(mockVerification, "mock verification.");
        }

        [Fact]
        public void DynamicDummyTypeIfVoidMethodIsCalledDoesNotThrows()
        {
            // Arrange
            bool mockVerification = false;
            var behaviorExpectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var dynamicInstance = (IDynamicInstanceTestType)GetParameters(c).ElementAt(0);
                    Assert.Null(Record.Exception(() => dynamicInstance.VoidMethod(null, 123)));
                    Assert.Null(Record.Exception(() => { dynamicInstance.Property = new object(); }));
                    mockVerification = true;
                }
            };
            var sut = new GuardClauseAssertion(new Fixture(), behaviorExpectation);
            var methodInfo = typeof(DynamicInstanceTestConstraint<>).GetMethod("Method");
            // Act
            sut.Verify(methodInfo);
            // Assert
            Assert.True(mockVerification, "mock verification.");
        }

        [Fact]
        public void VerifyMethodOnGenericManyTimeLoadsOnlyUniqueAssemblies()
        {
            // Arrange
            var sut = new GuardClauseAssertion(new Fixture());
            MethodInfo methodInfo = typeof(NoContraint<>).GetMethod(nameof(NoContraint<object>.Method));

            var assembliesBefore = AppDomain.CurrentDomain.GetAssemblies();

            // Act
            sut.Verify(methodInfo);
            sut.Verify(methodInfo);
            sut.Verify(methodInfo);
            // Assert
            var assembliesAfter = AppDomain.CurrentDomain.GetAssemblies();

            Assert.Equal(assembliesBefore, assembliesAfter);
        }

        private class NoContraint<T>
        {
            public NoContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class InterfacesContraint<T>
            where T : IInterfaceTestType, IEnumerable<object>
        {
            public InterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class StructureAndInterfacesContraint<T>
            where T : struct, IInterfaceTestType, IEnumerable<object>
        {
            public StructureAndInterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        public interface IInterfaceTestType
        {
            event EventHandler TestEvent;

            object Property { get; set; }

            void Method(object argument);

            TResult GenericMethod<TValue, TResult>(TValue argument)
                where TValue : ICloneable
                where TResult : class, ICloneable;
        }

        private class ClassContraint<T>
            where T : class
        {
            public ClassContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class CertainClassContraint<T>
            where T : ConcreteType
        {
            public CertainClassContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class CertainClassAndInterfacesContraint<T>
            where T : ConcreteType, IInterfaceTestType, IEnumerable<object>
        {
            public CertainClassAndInterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class MultipleGenericArguments<T1, T2>
            where T1 : class
        {
            public MultipleGenericArguments(T1 argument1, T2 argument2)
            {
            }

            public T1 Property { get; set; }

            public void Method(T1 argument1, T2 argument2)
            {
            }
        }

        private class AbstractTypeAndInterfacesContraint<T>
            where T : AbstractTestType, IInterfaceTestType, IEnumerable<object>
        {
            public AbstractTypeAndInterfacesContraint(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        public abstract class AbstractTestType
        {
            public abstract event EventHandler TestEvent;

            protected abstract event EventHandler ProtectedTestEvent;

            public abstract object Property { get; set; }

            protected abstract object ProtectedProperty { get; set; }

            public abstract void Method(object argument);

            protected abstract void ProtectedMethod(object argument);
        }

        private class OpenGenericTestType<T> : OpenGenericTestTypeBase<T>
            where T : class
        {
            public OpenGenericTestType(T argument)
                : base(argument)
            {
            }
        }

        private class OpenGenericTestTypeBase<T>
        {
            public OpenGenericTestTypeBase(T argument)
            {
            }

            public T Property { get; set; }

            public void Method(T argument)
            {
            }
        }

        private class ConstructedGenericTestType<T> : ConstructedGenericTestTypeBase<string, T>
            where T : class
        {
            public ConstructedGenericTestType(string argument1, T argument2)
                : base(argument1, argument2)
            {
            }
        }

        private class ConstructedGenericTestTypeBase<T1, T2>
        {
            public ConstructedGenericTestTypeBase(T1 argument1, T2 argument2)
            {
            }

            public T1 Property1 { get; set; }

            public T2 Property2 { get; set; }

            public void Method(T1 argument1, T2 argument2)
            {
            }
        }

        private class ParameterizedConstructorTestConstraint<T>
            where T : ParameterizedConstructorTestType, new()
        {
            public void Method(T argument, object test)
            {
                if (argument == null) throw new ArgumentNullException(nameof(argument));
                if (argument.Argument1 == null || argument.Argument2 == null)
                {
                    throw new ArgumentException("The constructor of the base type should be called with anonymous values.");
                }

                if (test == null)
                {
                    throw new ArgumentNullException(nameof(test));
                }
            }
        }

        public class ParameterizedConstructorTestType
        {
            // to test duplicating with the SpecimenBuilder field of a dummy type.
            public static ISpecimenBuilder SpecimenBuilder = null;

            public ParameterizedConstructorTestType(object argument1, string argument2)
            {
                this.Argument1 = argument1;
                this.Argument2 = argument2;
            }

            public object Argument1 { get; }

            public string Argument2 { get; }
        }

        private class InternalProtectedConstructorTestConstraint<T>
            where T : InternalProtectedConstructorTestType
        {
            public InternalProtectedConstructorTestConstraint(T argument)
            {
            }
        }

        private class ModestConstructorTestConstraint<T>
            where T : ModestConstructorTestType
        {
            public ModestConstructorTestConstraint(T argument)
            {
            }
        }

        public abstract class ModestConstructorTestType
        {
            protected ModestConstructorTestType(object argument1, int argument2)
            {
                throw new InvalidOperationException("Should use the modest constructor.");
            }

            protected ModestConstructorTestType(object argument1, string argument2, int argument3)
            {
                throw new InvalidOperationException("Should use the modest constructor.");
            }

            protected ModestConstructorTestType(object argument)
            {
            }
        }

        public class NoAccessibleConstructorTestType
        {
            private NoAccessibleConstructorTestType()
            {
            }
        }

        private class NoAccessibleConstructorTestConstraint<T>
            where T : NoAccessibleConstructorTestType
        {
            public NoAccessibleConstructorTestConstraint(T argument)
            {
            }
        }

        public class DynamicInstanceTestConstraint<T>
            where T : IDynamicInstanceTestType
        {
            public void Method(T argument)
            {
            }
        }

        public interface IDynamicInstanceTestType
        {
            object Property { get; set; }

            int VoidMethod(object argument1, int argument2);

            int ReturnMethod(object argument1, int argument2);
        }

        private class UnclosedGenericMethodTestType<T1>
            where T1 : class
        {
            public void Method<T2, T3, T4>(T1 argument1, int argument2, T2 argument3, T3 argument4, T4 argument5)
                where T2 : class
                where T4 : class
            {
                if (argument1 == null)
                {
                    throw new ArgumentNullException(nameof(argument1));
                }

                if (argument3 == null)
                {
                    throw new ArgumentNullException(nameof(argument3));
                }

                if (argument5 == null)
                {
                    throw new ArgumentNullException(nameof(argument5));
                }
            }
        }

        private class ConstructorMatchTestType<T1, T2>
            where T1 : class
        {
            public ConstructorMatchTestType(T1 argument)
            {
            }

            public ConstructorMatchTestType(T1 argument1, T2 argument2)
            {
            }

            public ConstructorMatchTestType(T1 argument1, T1 argument2)
            {
            }

            public ConstructorMatchTestType(T2 argument1, object argument2)
            {
            }
        }

        private class MethodMatchTestType<T1, T2>
            where T1 : class
        {
            public MethodMatchTestType(T1 argument)
            {
            }

            public void Method(T1 argument)
            {
            }

            public void Method(T1 argument1, T2 argument2)
            {
            }

            public void Method(T2 argument1, object argument2)
            {
            }

            public void Method<T3>(T1 argument1, object argument2)
                where T3 : class
            {
            }

            public void Method<T3>(int argument1, T3 argument2)
                where T3 : class
            {
            }

            public void Method<T3>(T1 argument1, T3 argument2)
                where T3 : class
            {
            }
        }

        private class ByRefTestType<T1>
            where T1 : class
        {
            public ByRefTestType(T1 argument)
            {
            }

            public void Method(ref T1 argument)
            {
            }

            public void Method<T2>(ref T2 argument)
                where T2 : class
            {
            }

            public void Method(ref T1 argument1, int argument2)
            {
            }

            public void Method(T1 argument1, int argument2)
            {
            }
        }

        private class NestedGenericParameterTestType<T1, T2>
        {
            public NestedGenericParameterTestType(IEnumerable<T1> arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(IEnumerable<IEnumerable<T2>> arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1 arg1, Func<T1, IEnumerable<T2>> arg2)
            {
                if (arg2 == null) throw new ArgumentNullException(nameof(arg2));
            }

            public NestedGenericParameterTestType(T1[] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1[,] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1[][] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(T1[,][][] arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
            }

            public NestedGenericParameterTestType(Func<T1, IEnumerable<IEnumerable<T2>>, T1[][]> arg1, T2 arg2)
            {
                if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            }
        }
    }
}