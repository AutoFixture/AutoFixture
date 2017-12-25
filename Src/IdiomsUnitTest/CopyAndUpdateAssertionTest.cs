using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Albedo;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class CopyAndUpdateAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new CopyAndUpdateAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new CopyAndUpdateAssertion(expectedComposer);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        public void ComparerIsCorrect()
        {
            // Arrange
            var dummyComposer = new Fixture();
            IEqualityComparer expected = new MemberInfoEqualityComparer();
            var sut = new CopyAndUpdateAssertion(dummyComposer, expected);
            // Act
            IEqualityComparer actual = sut.Comparer;
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new CopyAndUpdateAssertion(null));
        }

        [Fact]
        public void ConstructWithNullComparerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new CopyAndUpdateAssertion(new Fixture(), comparer: null));
        }

        [Fact]
        public void ParameterMemberMatcherIsCorrect()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var dummyComparer = new DummyEqualityComparer<object>();
            var expectedMatcher = new DummyReflectionElementComparer();
            var sut = new ConstructorInitializedMemberAssertion(
                dummyComposer, dummyComparer, expectedMatcher);
            // Act
            IEqualityComparer<IReflectionElement> result = sut.ParameterMemberMatcher;
            // Assert
            Assert.Equal(expectedMatcher, result);
        }

        [Fact]
        public void ConstructWithNullParameterMemberMatcherThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var dummyComparer = new DummyEqualityComparer<object>();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ConstructorInitializedMemberAssertion(dummyComposer, dummyComparer, null));
        }

        [Fact]
        public void VerifyNullMethodInfoThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new CopyAndUpdateAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo) null));
        }

        [Theory]
        [InlineData(typeof (ImmutableWellBehavedCopyMethods), "WithFirst")]
        [InlineData(typeof (ImmutableWellBehavedCopyMethods), "WithSecond")]
        [InlineData(typeof (ImmutableWellBehavedCopyMethods), "WithThird")]
        [InlineData(typeof (MutableWellBehavedCopyMethods), "WithFirst")]
        [InlineData(typeof (MutableWellBehavedCopyMethods), "WithSecond")]
        [InlineData(typeof (MutableWellBehavedCopyMethods), "WithThird")]
        public void VerifyWellBehavedDoesNotThrow(Type typeWithCopyUpdateMethod, string copyUpdateMethodName)
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new CopyAndUpdateAssertion(dummyComposer);
            var method = typeWithCopyUpdateMethod.GetMethod(copyUpdateMethodName);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(method)));
        }

        [Theory]
        [InlineData(typeof (MutableIllBehavedCopyMethods), "WithFirstButFirstDifferent", "First")]
        [InlineData(typeof (MutableIllBehavedCopyMethods), "WithSecondButSecondDifferent", "Second")]
        [InlineData(typeof (MutableIllBehavedCopyMethods), "WithThirdButThirdDifferent", "Third")]
        [InlineData(typeof (ImmutableIllBehavedCopyMethods), "WithFirstButSecondDefault", "Second")]
        [InlineData(typeof (ImmutableIllBehavedCopyMethods), "WithSecondButThirdDefault", "Third")]
        [InlineData(typeof (ImmutableIllBehavedCopyMethods), "WithThirdButFirstDefault", "First")]
        [InlineData(typeof (MutableIllBehavedCopyMethods), "WithFirstButSecondDefault", "Second")]
        [InlineData(typeof (MutableIllBehavedCopyMethods), "WithSecondButThirdDefault", "Third")]
        [InlineData(typeof (MutableIllBehavedCopyMethods), "WithThirdButFirstDefault", "First")]
        public void VerifyIllBehavedWithInvalidMemberValueThrows(
            Type typeWithCopyUpdateMethod,
            string copyUpdateMethodName,
            string expectedMemberNameWithInvalidValue)
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new CopyAndUpdateAssertion(dummyComposer);
            var method = typeWithCopyUpdateMethod.GetMethod(copyUpdateMethodName);
            var member = typeWithCopyUpdateMethod.GetMember(expectedMemberNameWithInvalidValue).Single();
            // Act & Assert
            var e = Assert.Throws<CopyAndUpdateException>(() =>
                sut.Verify(method));
            AssertExceptionPropertiesEqual(e, method, memberWithInvalidValue: member);
        }

        [Theory]
        [InlineData(typeof (ImmutableWithDifferentName), "With", "differentName")]
        [InlineData(typeof (ImmutableWithDifferentType), "With", "differentType")]
        [InlineData(typeof (ImmutableWithDifferentBoth), "With", "differentBoth")]
        public void VerifyWhenMethodHasNoMatchingPublicMembersThrows(
            Type copyUpdateMethodType,
            string methodName,
            string argumentNameWithNoMatchingPublicMember)
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new CopyAndUpdateAssertion(dummyComposer);
            var method = copyUpdateMethodType.GetMethod(methodName);
            var parameter = method.GetParameters().Single(p =>
                p.Name == argumentNameWithNoMatchingPublicMember);
            // Act & Assert
            var e = Assert.Throws<CopyAndUpdateException>(() =>
                sut.Verify(method));
            AssertExceptionPropertiesEqual(e, method, parameter);
        }

        private class ImmutableWellBehavedCopyMethods
        {
            public int First { get; private set; }
            public string Second { get; private set; }
            public ComplexMutable<int, int, int> Third { get; private set; }

            public ImmutableWellBehavedCopyMethods(int first, string second, ComplexMutable<int, int, int> third)
            {
                this.First = first;
                this.Second = second;
                this.Third = third;
            }

            public ImmutableWellBehavedCopyMethods WithFirst(int first)
            {
                return new ImmutableWellBehavedCopyMethods(first, this.Second, this.Third);
            }

            public ImmutableWellBehavedCopyMethods WithSecond(string second)
            {
                return new ImmutableWellBehavedCopyMethods(this.First, second, this.Third);
            }

            public ImmutableWellBehavedCopyMethods WithThird(ComplexMutable<int, int, int> third)
            {
                return new ImmutableWellBehavedCopyMethods(this.First, this.Second, third);
            }
        }

        private class MutableWellBehavedCopyMethods
        {
            public int First;
            public string Second;
            public ComplexMutable<int, int, int> Third;

            public MutableWellBehavedCopyMethods(int first, string second, ComplexMutable<int, int, int> third)
            {
                this.First = first;
                this.Second = second;
                this.Third = third;
            }

            public MutableWellBehavedCopyMethods WithFirst(int first)
            {
                return new MutableWellBehavedCopyMethods(first, this.Second, this.Third);
            }

            public MutableWellBehavedCopyMethods WithSecond(string second)
            {
                return new MutableWellBehavedCopyMethods(this.First, second, this.Third);
            }

            public MutableWellBehavedCopyMethods WithThird(ComplexMutable<int, int, int> third)
            {
                return new MutableWellBehavedCopyMethods(this.First, this.Second, third);
            }
        }

        private class ImmutableIllBehavedCopyMethods
        {
            public int First { get; private set; }
            public string Second { get; private set; }
            public ComplexMutable<int, int, int> Third { get; private set; }

            public ImmutableIllBehavedCopyMethods(int first, string second, ComplexMutable<int, int, int> third)
            {
                this.First = first;
                this.Second = second;
                this.Third = third;
            }

            public ImmutableIllBehavedCopyMethods WithFirstButSecondDefault(int first)
            {
                return new ImmutableIllBehavedCopyMethods(first, default(string), this.Third);
            }

            public ImmutableIllBehavedCopyMethods WithSecondButThirdDefault(string second)
            {
                return new ImmutableIllBehavedCopyMethods(this.First, second, default(ComplexMutable<int, int, int>));
            }

            public ImmutableIllBehavedCopyMethods WithThirdButFirstDefault(ComplexMutable<int, int, int> third)
            {
                return new ImmutableIllBehavedCopyMethods(default(int), this.Second, third);
            }
        }

        private class MutableIllBehavedCopyMethods
        {
            public int First;
            public string Second;
            public ComplexMutable<int, int, int> Third;

            public MutableIllBehavedCopyMethods(int first, string second, ComplexMutable<int, int, int> third)
            {
                this.First = first;
                this.Second = second;
                this.Third = third;
            }

            public MutableIllBehavedCopyMethods WithFirstButFirstDifferent(int first)
            {
                return new MutableIllBehavedCopyMethods(first + 1, this.Second, this.Third);
            }

            public MutableIllBehavedCopyMethods WithSecondButSecondDifferent(string second)
            {
                return new MutableIllBehavedCopyMethods(this.First, second + "extra unexpected", this.Third);
            }

            public MutableIllBehavedCopyMethods WithThirdButThirdDifferent(ComplexMutable<int, int, int> third)
            {
                var differentThird = new ComplexMutable<int, int, int>
                {
                    First = third.First+1,
                    Second = third.Second,
                    Third = third.Third,
                };
                return new MutableIllBehavedCopyMethods(this.First, this.Second, differentThird);
            }

            public MutableIllBehavedCopyMethods WithFirstButSecondDefault(int first)
            {
                return new MutableIllBehavedCopyMethods(first, default(string), this.Third);
            }

            public MutableIllBehavedCopyMethods WithSecondButThirdDefault(string second)
            {
                return new MutableIllBehavedCopyMethods(this.First, second, default(ComplexMutable<int, int, int>));
            }

            public MutableIllBehavedCopyMethods WithThirdButFirstDefault(ComplexMutable<int, int, int> third)
            {
                return new MutableIllBehavedCopyMethods(default(int), this.Second, third);
            }
        }

        private class ImmutableWithDifferentName
        {
            public int SameName { get; private set; }
            public int DifferentNameX { get; private set; }

            public ImmutableWithDifferentName(int sameName, int differentName)
            {
                this.SameName = sameName;
                this.DifferentNameX = differentName;
            }

            public ImmutableWithDifferentName With(int differentName)
            {
                return new ImmutableWithDifferentName(this.SameName, differentName);
            }
        }

        private class ImmutableWithDifferentType
        {
            public int SameType { get; private set; }
            public string DifferentType { get; private set; }

            public ImmutableWithDifferentType(int sameType, int differentType)
            {
                this.SameType = sameType;
                this.DifferentType = differentType.ToString(CultureInfo.InvariantCulture);
            }

            public ImmutableWithDifferentType With(int differentType)
            {
                return new ImmutableWithDifferentType(this.SameType, differentType);
            }
        }

        private class ImmutableWithDifferentBoth
        {
            public int SameBoth { get; private set; }
            public string DifferentBothX { get; private set; }

            public ImmutableWithDifferentBoth(int sameBoth, int differentBoth)
            {
                this.SameBoth = sameBoth;
                this.DifferentBothX = differentBoth.ToString(CultureInfo.InvariantCulture);
            }

            public ImmutableWithDifferentBoth With(int sameBoth, int differentBoth)
            {
                return new ImmutableWithDifferentBoth(sameBoth, differentBoth);
            }
        }

        private class ComplexMutable<TFirst, TSecond, TThird>
        {
            public TFirst First { get; set; }
            public TSecond Second { get; set; }
            public TThird Third { get; set; }
        }

        private static void AssertExceptionPropertiesEqual(
            CopyAndUpdateException ex,
            MethodInfo copyAndUpdateMethod,
            ParameterInfo argumentWithNoMatchingPublicMember = null,
            MemberInfo memberWithInvalidValue = null)
        {
            Assert.Equal(copyAndUpdateMethod, ex.MethodInfo);
            Assert.Equal(argumentWithNoMatchingPublicMember, ex.ArgumentWithNoMatchingPublicMember);
            Assert.Equal(memberWithInvalidValue, ex.MemberWithInvalidValue);
        }

        class DummyReflectionElementComparer : IEqualityComparer<IReflectionElement>
        {
            public bool Equals(IReflectionElement x, IReflectionElement y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(IReflectionElement obj)
            {
                throw new NotImplementedException();
            }
        }

        class DummyEqualityComparer<T> : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                throw new NotImplementedException();
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
