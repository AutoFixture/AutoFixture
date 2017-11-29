﻿using System;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class ModestConstructorQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new ModestConstructorQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new ModestConstructorQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromTypeWithNoPublicConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ModestConstructorQuery();
            var typeWithNoPublicConstructors = typeof(AbstractType);
            // Exercise system
            var result = sut.SelectMethods(typeWithNoPublicConstructors);
            // Verify outcome
            Assert.False(result.Any());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SingleParameterType<object>))]
        [InlineData(typeof(ConcreteType))]
        [InlineData(typeof(MultiUnorderedConstructorType))]
        public void SelectMethodsFromTypeReturnsCorrectResult(Type type)
        {
            // Fixture setup
            var expectedConstructors = from ci in type.GetConstructors()
                                       let parameters = ci.GetParameters()
                                       orderby parameters.Length ascending
                                       select new ConstructorMethod(ci) as IMethod;

            var sut = new ModestConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(type);
            // Verify outcome
            Assert.True(expectedConstructors.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void DoesNotReturnConstructorsWithParametersOfEnclosingType()
        {
            // Fixture setup
            var sut = new ModestConstructorQuery();
            // Exercise system
            var result = sut.SelectMethods(typeof(TypeWithCopyConstructorsOnly));
            // Verify outcome
            Assert.Empty(result);
            // Teardown
        }

        public class TypeWithCopyConstructorsOnly
        {
            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other)
            {
            }

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, int num)
            {
            }

            public TypeWithCopyConstructorsOnly(TypeWithCopyConstructorsOnly other, string str)
            {
            }
        }
    }
}
