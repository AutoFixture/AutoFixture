using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class CompositeDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<DataAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute((IEnumerable<DataAttribute>)null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null, dummyTypes).ToList());
            // Teardown
        }

        [Fact]
        public void GetDataWithNullTypesThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            Action a = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(a.Method, null).ToList());
            // Teardown
        }

        [Fact]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var result = sut.GetData(a.Method, Type.EmptyTypes);
            Array.ForEach(result.ToArray(), Assert.Empty);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(SufficientDataSource))]
        public void GetDataReturnsCorrectResult(IEnumerable<DataAttribute> attributes, IEnumerable<object[]> expectedResult)
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();
            // Exercise system
            var result = new CompositeDataAttribute(attributes).GetData(method, parameterTypes).ToList();
            // Verify outcome 
            Assert.True(expectedResult.SequenceEqual(result, new TheoryComparer()));
            // Teardown
        }

        [Theory]
        [ClassData(typeof(InsufficientDataSource))]
        public void GetDataThrows(IEnumerable<DataAttribute> attributes)
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(
                () => { new CompositeDataAttribute(attributes).GetData(method, parameterTypes).Any(); }
                );
            // Teardown
        }

        private sealed class TheoryComparer : IEqualityComparer<object[]>
        {
            public bool Equals(object[] x, object[] y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(object[] array)
            {
                int hashCode = 0;

                foreach (object obj in array)
                {
                    hashCode += obj.GetHashCode();
                }

                return hashCode;
            }
        }

        private sealed class SufficientDataSource : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var testCases = from method
                                    in this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                where method.ReturnType == typeof(object[])
                                select (Func<object[]>)Delegate.CreateDelegate(typeof(Func<object[]>), this, method);

                foreach (var testCase in testCases)
                {
                    yield return testCase();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private object[] TestCase1()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2, 3 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 2, 3 } };

                return new object[] { attributes, expectedResult };
            }

            private object[] TestCase2()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2, 3 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 4, 5, 6 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 2, 3 } };

                return new object[] { attributes, expectedResult };
            }

            private object[] TestCase3()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2, 3, 4 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 2, 3 } };

                return new object[] { attributes, expectedResult };
            }

            private object[] TestCase4()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 2, 3, 4 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 3, 4 } };

                return new object[] { attributes, expectedResult };
            }

            private object[] TestCase5()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 3, 4, 5 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 2, 5 } };

                return new object[] { attributes, expectedResult };
            }

            private object[] TestCase6()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 3, 4, 5 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 2, 5 } };

                return new object[] { attributes, expectedResult };
            }

            private object[] TestCase7()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } })
                };

                IEnumerable<object[]> expectedResult = new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } };

                return new object[] { attributes, expectedResult };
            }
        }

        private sealed class InsufficientDataSource : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var testCases = from method
                                    in this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                where method.ReturnType == typeof(object[])
                                select (Func<object[]>)Delegate.CreateDelegate(typeof(Func<object[]>), this, method);

                foreach (var testCase in testCases)
                {
                    yield return testCase();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private object[] TestCase1()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 3, 4 } })
                };

                return new object[] { attributes };
            }

            private object[] TestCase2()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 2, 3 } })
                };

                return new object[] { attributes };
            }

            private object[] TestCase3()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 2, 3 } })
                };

                return new object[] { attributes };
            }

            private object[] TestCase4()
            {
                var method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                var parameterTypes = (from pi in parameters
                                      select pi.ParameterType).ToArray();

                var attributes = new DataAttribute[]
                {
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 2 } }),
                    new FakeDataAttribute(method, parameterTypes, new[] { new object[] { 3 } })
                };

                return new object[] { attributes };
            }
        }
    }
}