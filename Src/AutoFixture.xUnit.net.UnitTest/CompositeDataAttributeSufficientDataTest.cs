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
    public class CompositeDataAttributeSufficientDataTest : IEnumerable<object[]>
    {
        private readonly MethodInfo method;
        private readonly Type[] parameterTypes;

        public CompositeDataAttributeSufficientDataTest()
        {
            this.method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = method.GetParameters();
            this.parameterTypes = (from pi in parameters
                                   select pi.ParameterType).ToArray();
        }

        [Theory]
        [ClassData(typeof(CompositeDataAttributeSufficientDataTest))]
        public void GetDataReturnsCorrectResult(IEnumerable<DataAttribute> attributes, IEnumerable<object[]> expectedResult)
        {
            // Fixture setup
            // Exercise system
            var result = new CompositeDataAttribute(attributes).GetData(this.method, this.parameterTypes).ToList();
            // Verify outcome 
            Assert.True(expectedResult.SequenceEqual(result, new TheoryComparer()));
            // Teardown
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 } 
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 4, 5, 6 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1       } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3, 4 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 3, 4 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2    } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3, 4, 5 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 5 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 }, new object[] { 4,  5, 6 }                          }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 7, 8    }, new object[] { 9, 10    }, new object[] { 11, 12 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2    }, new object[] {  3,  4     }, new object[] {  5,  6     } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 7, 8, 9 }, new object[] { 10, 11, 12 }, new object[] { 13, 14, 15 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 9 }, new object[] { 3, 4, 12 }, new object[] { 5, 6, 15 }
                    }
            );

            // Second attribute restricts
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 7, 8, 9 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }
                    }
            );

            // Shortest data provider is limiting factor
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 4, 5, 6 }, new object[] { 7, 8, 9 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }
                    }
            );

            // Test incorrect number of parameters - should just return what it's given
            // and let xUnit deal with counting parameters
            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3, 4 } })
                },
                expected: new[]
                {
                    new object[] { 1, 2 }
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3 } })
                },
                expected: new[]
                {
                    new object[] { 1, 3 },
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {      } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3 } })
                },
                expected: new[]
                {
                    new object[] { 1, 3 },
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3 } })
                },
                expected: new[]
                {
                    new object[] { 1 }
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3, 4 } }),
                },
                expected: new[]
                {
                    new object[] { 1, 2, 3, 4 }
                }
            );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static object[] CreateTestCase(object[] data, object[] expected)
        {
            return new object[] { data, expected };
        }

        private sealed class TheoryComparer : IEqualityComparer<object[]>
        {
            public bool Equals(object[] x, object[] y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(object[] array)
            {
                return (from obj in array select obj.GetHashCode()).Aggregate((x, y) => x ^ y);
            }
        }
    }
}
