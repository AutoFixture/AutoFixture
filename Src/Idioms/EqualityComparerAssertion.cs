using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    public class EqualityComparerAssertion : CompositeIdiomaticAssertion
    {
        public EqualityComparerAssertion(ISpecimenBuilder builder) : base(CreateChildAssertions(builder)) { }

        private static IEnumerable<IIdiomaticAssertion> CreateChildAssertions(ISpecimenBuilder builder)
        {
            yield return new EqualityComparerEqualsSelfAssertion(builder);

            yield return new EqualityComparerEqualsSymmetricAssertion(builder);

            yield return new EqualityComparerEqualsTransitiveAssertion(builder);

            yield return new EqualityComparerGetHashCodeAssertion(builder);

            yield return new EqualityComparerEqualsNullAssertion(builder);
        }
    }

    public class EqualityComparerGetHashCodeAssertion : IdiomaticAssertion
    {
        public ISpecimenBuilder Builder { get; }

        public EqualityComparerGetHashCodeAssertion(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo is { Name: nameof(IEqualityComparer.GetHashCode), ReflectedType: { } type } && methodInfo.GetParameters().Length == 1 && type.ImplementsGenericInterfaceDefinition(typeof(IEqualityComparer<>)))
            {
                var argumentType = methodInfo.GetParameters()[0].ParameterType;

                if (!methodInfo.ReflectedType.ImplementsGenericInterface(typeof(IEqualityComparer<>), argumentType))
                {
                    return;
                }

                var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

                var testSubject = this.Builder.CreateAnonymous(argumentType);

                var firstHashCode = (int)methodInfo.Invoke(comparer, new[] { testSubject });

                var secondHashCode = (int)methodInfo.Invoke(comparer, new[] { testSubject });

                if (firstHashCode != secondHashCode)
                {
                    throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                        "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                        "calling GetHashCode(x) should always return same value.",
                        type.FullName));
                }
            }
        }
    }

    public abstract class EqualityComparerEqualsAssertion : IdiomaticAssertion
    {
        public ISpecimenBuilder Builder { get; }

        protected EqualityComparerEqualsAssertion(ISpecimenBuilder builder)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo is { Name: nameof(IEqualityComparer.Equals), ReflectedType: { } type } && methodInfo.GetParameters().Length == 2 && type.ImplementsGenericInterfaceDefinition(typeof(IEqualityComparer<>)))
            {
                var argumentType = methodInfo.GetParameters()[0].ParameterType;

                if (!methodInfo.ReflectedType.ImplementsGenericInterface(typeof(IEqualityComparer<>), argumentType))
                {
                    return;
                }

                this.VerifyEquals(methodInfo, argumentType);
            }
        }

        protected abstract void VerifyEquals(MethodInfo methodInfo, Type argumentType);
    }

    public class EqualityComparerEqualsSelfAssertion : EqualityComparerEqualsAssertion
    {
        public EqualityComparerEqualsSelfAssertion(ISpecimenBuilder builder) : base(builder) { }

        protected override void VerifyEquals(MethodInfo methodInfo, Type argumentType)
        {
            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

            var testSubject = this.Builder.CreateAnonymous(argumentType);

            var result = (bool)methodInfo.Invoke(comparer, new[] { testSubject, testSubject });

            if (!result)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                        "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                        "calling Equals(x, x) should return true.",
                        methodInfo.ReflectedType!.FullName));
            }
        }
    }

    public class EqualityComparerEqualsSymmetricAssertion : EqualityComparerEqualsAssertion
    {
        public EqualityComparerEqualsSymmetricAssertion(ISpecimenBuilder builder) : base(builder) { }

        protected override void VerifyEquals(MethodInfo methodInfo, Type argumentType)
        {
            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

            var firstTestSubject = this.Builder.CreateAnonymous(argumentType);

            var secondTestSubject = this.Builder.CreateAnonymous(argumentType);

            var directResult = (bool)methodInfo.Invoke(comparer, new[] { firstTestSubject, secondTestSubject });

            var invertedResult = (bool)methodInfo.Invoke(comparer, new[] { secondTestSubject, firstTestSubject });

            if (directResult != invertedResult)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                        "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                        "calling Equals(x, y) should return same as Equals(y, x).",
                        methodInfo.ReflectedType!.FullName));
            }
        }
    }

    public class EqualityComparerEqualsTransitiveAssertion : EqualityComparerEqualsAssertion
    {
        public EqualityComparerEqualsTransitiveAssertion(ISpecimenBuilder builder) : base(builder) { }

        protected override void VerifyEquals(MethodInfo methodInfo, Type argumentType)
        {
            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

            var firstTestSubject = this.Builder.CreateAnonymous(argumentType);

            var secondTestSubject = this.Builder.CreateAnonymous(argumentType);

            var thirdTestSubject = this.Builder.CreateAnonymous(argumentType);

            var firstResult = (bool)methodInfo.Invoke(comparer, new[] { firstTestSubject, secondTestSubject });

            var secondResult = (bool)methodInfo.Invoke(comparer, new[] { secondTestSubject, thirdTestSubject });

            var thirdResult = (bool)methodInfo.Invoke(comparer, new[] { firstTestSubject, thirdTestSubject });

            if (firstResult != secondResult || firstResult != thirdResult)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                        "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                        "calling Equals(x, z) should return same as Equals(x, y) and Equals(y, z).",
                        methodInfo.ReflectedType!.FullName));
            }
        }
    }

    public class EqualityComparerEqualsNullAssertion : EqualityComparerEqualsAssertion
    {
        public EqualityComparerEqualsNullAssertion(ISpecimenBuilder builder) : base(builder) { }

        protected override void VerifyEquals(MethodInfo methodInfo, Type argumentType)
        {
            if (methodInfo.ReflectedType!.IsValueType)
            {
                return;
            }

            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

            var testSubject = this.Builder.CreateAnonymous(argumentType);

            var result = (bool)methodInfo.Invoke(comparer, new[] { testSubject, null });

            if (result)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                    "calling Equals(x, null) should return false.",
                    methodInfo.ReflectedType!.FullName));
            }
        }
    }

    public class EqualityComparerEqualsNullNullAssertion : EqualityComparerEqualsAssertion
    {
        public EqualityComparerEqualsNullNullAssertion(ISpecimenBuilder builder) : base(builder) { }

        protected override void VerifyEquals(MethodInfo methodInfo, Type argumentType)
        {
            if (methodInfo.ReflectedType!.IsValueType)
            {
                return;
            }

            var comparer = this.Builder.CreateAnonymous(methodInfo.ReflectedType);

            var result = (bool)methodInfo.Invoke(comparer, new object[] { null, null });

            if (!result)
            {
                throw new EqualityComparerImplementationException(string.Format(CultureInfo.CurrentCulture,
                    "The type '{0}' implements the `IEqualityComparer<T>` interface incorrectly: " +
                    "calling Equals(null, null) should return true.",
                    methodInfo.ReflectedType!.FullName));
            }
        }
    }

    public class EqualityComparerImplementationException : Exception
    {
        public EqualityComparerImplementationException(string message) : base(message) { }
    }

    internal static class TypeExtensions
    {
        public static bool ImplementsGenericInterfaceDefinition(this Type type, Type interfaceType) => type.GetInterfaces().Any(i => i.GetGenericTypeDefinition().IsAssignableFrom(interfaceType));

        public static bool ImplementsGenericInterface(this Type type, Type interfaceType, params Type[] argumentTypes) => type.GetInterfaces().Any(i => i.IsAssignableFrom(interfaceType.MakeGenericType(argumentTypes)));
    }
}