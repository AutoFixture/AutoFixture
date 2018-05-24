﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestTypeFoundation;
using Xunit;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    public class CompositeDataAttributeSufficientDataTest : IEnumerable<object[]>
    {
        private readonly MethodInfo method;

        public CompositeDataAttributeSufficientDataTest()
        {
            this.method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = this.method.GetParameters();
        }

        [Theory]
        [ClassData(typeof(CompositeDataAttributeSufficientDataTest))]
        public void GetDataReturnsCorrectResult(IEnumerable<DataAttribute> attributes, IEnumerable<object[]> expectedResult)
        {
            // Arrange
            // Act
            var result = new CompositeDataAttribute(attributes.ToArray()).GetData(this.method).ToList();
            // Assert
            Assert.True(expectedResult.SequenceEqual(result, new TheoryComparer()));
        }

        public IEnumerator<object[]> GetEnumerator()
        {
#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 3 }
                    });

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 4, 5, 6 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 3 }
                    });

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1       } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 2, 3, 4 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 3, 4 }
                    });

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2    } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 3, 4, 5 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 5 }
                    });

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    });

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 }, new object[] { 4,  5, 6 }                          }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 7, 8    }, new object[] { 9, 10    }, new object[] { 11, 12 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    });

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2    }, new object[] {  3,  4     }, new object[] {  5,  6     } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 7, 8, 9 }, new object[] { 10, 11, 12 }, new object[] { 13, 14, 15 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 9 }, new object[] { 3, 4, 12 }, new object[] { 5, 6, 15 }
                    });

            // Second attribute restricts
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 7, 8, 9 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 3 }
                    });

            // Shortest data provider is limiting factor
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 4, 5, 6 }, new object[] { 7, 8, 9 } })
                    },
                expected: new[]
                    {
                        new object[] { 1, 2, 3 }
                    });

            // Test incorrect number of parameters - should just return what it's given
            // and let xUnit deal with counting parameters
            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(this.method, new[] { new object[] { 3, 4 } })
                },
                expected: new[]
                {
                    new object[] { 1, 2 }
                });

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method, new[] { new object[] { 2, 3 } })
                },
                expected: new[]
                {
                    new object[] { 1, 3 },
                });

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method, new[] { new object[] {      } }),
                    new FakeDataAttribute(this.method, new[] { new object[] { 2, 3 } })
                },
                expected: new[]
                {
                    new object[] { 1, 3 },
                });

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(this.method, new[] { new object[] { 2 } }),
                    new FakeDataAttribute(this.method, new[] { new object[] { 3 } })
                },
                expected: new[]
                {
                    new object[] { 1 }
                });

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3, 4 } }),
                },
                expected: new[]
                {
                    new object[] { 1, 2, 3, 4 }
                });
#pragma warning restore SA1025 // Code should not contain multiple whitespace in a row
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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