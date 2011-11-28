using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyMethodQueryTest
    {
        [Fact]
        public void SutIsMethodQuery()
        {
            // Fixture setup
            // Exercise system
            var sut = new FakeItEasyMethodQuery();
            // Verify outcome
            Assert.IsAssignableFrom<IMethodQuery>(sut);
            // Teardown
        }

        [Fact]
        public void SelectMethodsFromNullTypeThrows()
        {
            // Fixture setup
            var sut = new FakeItEasyMethodQuery();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.SelectMethods(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(AbstractType))]
        public void SelectReturnsCorrectResultForNonInterfaces(Type t)
        {
            // Fixture setup
            var expectedMethods = (from ci in t.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        let paramInfos = ci.GetParameters()
                                        orderby paramInfos.Length ascending
                                        select new FakeItEasyMethod(ci.DeclaringType, paramInfos) as IMethod);
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.True(expectedMethods.SequenceEqual(result, new MethodComparer()));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectReturnsCorrectNumberOfMethodsForInterface(Type t)
        {
            // Fixture setup
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            Assert.Equal(1, result.Count());
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectReturnsCorrectResultForInterface(Type t)
        {
            // Fixture setup
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var method = Assert.IsAssignableFrom<FakeItEasyMethod>(result.Single());
            Assert.Equal(t, method.TargetType);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void SelectReturnsResultWithNoParametersForInterface(Type t)
        {
            // Fixture setup
            var sut = new FakeItEasyMethodQuery();
            // Exercise system
            var result = sut.SelectMethods(t);
            // Verify outcome
            var method = Assert.IsAssignableFrom<FakeItEasyMethod>(result.Single());
            Assert.Empty(method.Parameters);
            // Teardown
        }

        private sealed class MethodComparer : IEqualityComparer<IMethod>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first  IMethod type to compare.</param>
            /// <param name="y">The second IMethod type to compare.</param>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(IMethod x, IMethod y)
            {
                return x.Parameters.SequenceEqual(y.Parameters);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data 
            /// structures like a hash table. 
            /// </returns>
            /// <exception cref="T:System.ArgumentNullException">
            /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/>
            /// is null.
            ///   </exception>
            public int GetHashCode(IMethod obj)
            {
                return obj.Parameters.GetHashCode();
            }
        }
    }
}